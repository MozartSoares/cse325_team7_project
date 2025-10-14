using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Api.Mappings;
using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var userId = User.GetUserIdOrThrow();
        var user = await _userService.Get(userId);
        return Ok(user.ToDto());
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] AuthRefreshDto dto)
    {
        var refreshed = await _authService.Refresh(dto.AccessToken);
        return Ok(refreshed.ToDto());
    }
}
