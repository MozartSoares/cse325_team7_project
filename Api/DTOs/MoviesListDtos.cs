namespace cse325_team7_project.Api.DTOs;

using System.ComponentModel.DataAnnotations;

public class MoviesListCreateDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public List<string>? Movies { get; set; } = new List<string>();
}

public class MoviesListUpdateDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(50)]
    public List<string>? Movies { get; set; } = new List<string>();
}

public record MoviesListResponseDto(
    string Id,
    string Title,
    List<string> Movies,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

