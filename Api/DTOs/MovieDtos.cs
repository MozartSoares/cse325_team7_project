using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace cse325_team7_project.Api.DTOs;

public class CastMemberDto
{
    public string Name { get; set; } = string.Empty;
    public CastRole Role { get; set; }
}

public record MovieCreateDto(
    string Title,
    DateOnly ReleaseDate,
    Genre Genre,
    string Description,
    string Studio,
    List<CastMemberDto> Cast,
    string Image,
    string ThumbnailImage,
    decimal Budget
);

public class MovieUpdateDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateOnly ReleaseDate { get; set; }

    [Required]
    public Genre Genre { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Studio { get; set; } = string.Empty;

    [Required]
    public List<CastMemberDto> Cast { get; set; } = new List<CastMemberDto>();

    [Url]
    public string Image { get; set; } = string.Empty;

    [Url]
    public string ThumbnailImage { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Budget { get; set; }
}

public record MovieResponseDto(
    string Id,
    string Title,
    DateOnly ReleaseDate,
    Genre Genre,
    string Description,
    string Studio,
    List<CastMember> Cast,
    string Image,
    string ThumbnailImage,
    decimal Budget,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

