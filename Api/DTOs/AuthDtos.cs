namespace cse325_team7_project.Api.DTOs;

public record AuthRegisterDto(
    string Username,
    string Name,
    string Email,
    string Password
);

public record AuthLoginDto(
    string UsernameOrEmail,
    string Password
);

public record AuthResponseDto(
    string AccessToken,
    DateTime ExpiresAt,
    UserResponseDto User
);
