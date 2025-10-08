using cse325_team7_project.Domain.Enums;

namespace cse325_team7_project.Api.DTOs;

public record UserCreateDto(
    string Username,
    string Name,
    string Email,
    string Password
);

//only used by admin
public record UserCreateAdminDto(
    string Username,
    string Name,
    string Email,
    string Password,
    UserRole Role
);

public record UserUpdateDto(
    string Name,
    string Email
);

public record UserResponseDto(
    string Id,
    string Username,
    string Name,
    string Email,
    List<string> Lists,
    UserRole Role,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
