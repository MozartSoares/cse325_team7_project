using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Domain.ValueObjects;

namespace cse325_team7_project.Api.DTOs;

public record CastMemberDto(string Name, CastRole Role);

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

public record MovieUpdateDto(
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

