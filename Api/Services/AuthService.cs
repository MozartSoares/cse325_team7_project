using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Api.Options;
using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace cse325_team7_project.Api.Services;

/// <summary>
/// Handles user registration, login, and JWT issuance.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IMongoCollection<User> _users;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserService userService,
        IMongoCollection<User> users,
        IPasswordHasher<User> passwordHasher,
        IOptions<JwtOptions> jwtOptions,
        ILogger<AuthService> logger)
    {
        _userService = userService;
        _users = users;
        _passwordHasher = passwordHasher;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_jwtOptions.Key))
        {
            throw new InvalidOperationException("JWT signing key is missing. Configure Jwt:Key before using AuthService.");
        }
    }

    public async Task<AuthResult> Register(string username, string name, string email, string password)
    {
        var normalizedUsername = Normalize(username);
        var normalizedName = Normalize(name);
        var normalizedEmail = Normalize(email);
        var normalizedPassword = Normalize(password);

        if (string.IsNullOrWhiteSpace(normalizedUsername))
            throw new ValidationException("Username is required.");
        if (string.IsNullOrWhiteSpace(normalizedName))
            throw new ValidationException("Name is required.");
        if (string.IsNullOrWhiteSpace(normalizedEmail))
            throw new ValidationException("Email is required.");
        if (string.IsNullOrWhiteSpace(normalizedPassword))
            throw new ValidationException("Password is required.");

        var user = new User
        {
            Username = normalizedUsername,
            Name = normalizedName,
            Email = normalizedEmail,
            Role = UserRole.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, normalizedPassword);

        var created = await _userService.Create(user);
        return BuildAuthResult(created);
    }

    public async Task<AuthResult> Login(string usernameOrEmail, string password)
    {
        var identifier = Normalize(usernameOrEmail);
        var normalizedPassword = Normalize(password);

        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(normalizedPassword))
        {
            throw new ValidationException("Username/email and password are required.");
        }

        var user = await FindByUsernameOrEmail(identifier);
        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, normalizedPassword);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var newHash = _passwordHasher.HashPassword(user, normalizedPassword);
            var update = Builders<User>.Update
                .Set(u => u.PasswordHash, newHash)
                .Set(u => u.UpdatedAt, DateTime.UtcNow);

            try
            {
                await _users.UpdateOneAsync(u => u.Id == user.Id, update);
                user.PasswordHash = newHash;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to persist password rehash for user {UserId}", user.Id);
            }
        }

        return BuildAuthResult(user);
    }

    private static string Normalize(string? value) => value?.Trim() ?? string.Empty;

    private async Task<User?> FindByUsernameOrEmail(string identifier)
    {
        var user = await _users
            .Find(u => u.Username.Equals(identifier, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefaultAsync();

        if (user is not null)
        {
            return user;
        }

        return await _users
            .Find(u => u.Email.Equals(identifier, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefaultAsync();
    }

    private AuthResult BuildAuthResult(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.Key);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
        var lifetimeMinutes = _jwtOptions.AccessTokenMinutes > 0 ? _jwtOptions.AccessTokenMinutes : 60;
        var expiresAt = DateTime.UtcNow.AddMinutes(lifetimeMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new("role", user.Role.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }

        if (!string.IsNullOrWhiteSpace(user.Name))
        {
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: signingCredentials
        );

        var serialized = handler.WriteToken(token);
        return new AuthResult(user, serialized, expiresAt);
    }
}
