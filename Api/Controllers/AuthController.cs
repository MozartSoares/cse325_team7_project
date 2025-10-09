using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Api.Mappings;
using cse325_team7_project.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IUserService userService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] AuthRegisterDto dto)
    {
        var (username, name, email, password) = dto.ToRegisterInput();
        var result = await _authService.Register(username, name, email, password);
        return Ok(result.ToDto());
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthLoginDto dto)
    {
        var (usernameOrEmail, password) = dto.ToLoginInput();
        var result = await _authService.Login(usernameOrEmail, password);
        return Ok(result.ToDto());
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> Me()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrWhiteSpace(userIdValue) || !ObjectId.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedException("Invalid user identity.");
        }

        var user = await _userService.Get(userId);
        return Ok(user.ToDto());
    }
}
