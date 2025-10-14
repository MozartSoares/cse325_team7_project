using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Domain.Models;

namespace cse325_team7_project.Api.Services.Interfaces;

/// <summary>
/// Provides user-facing authentication workflows (register/login) and token issuance.
/// </summary>
public interface IAuthService
{
    Task<AuthResult> Register(string username, string name, string email, string password);

    Task<AuthResult> Login(string usernameOrEmail, string password);

    Task<AuthResult> Refresh(string accessToken);
}

public record AuthResult(User User, string AccessToken, DateTime ExpiresAt);
