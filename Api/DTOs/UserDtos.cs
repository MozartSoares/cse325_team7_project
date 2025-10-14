using cse325_team7_project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

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

public class UserUpdateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
};

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
