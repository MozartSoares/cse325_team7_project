namespace cse325_team7_project.Api.DTOs;

public record MoviesListCreateDto(
    string Title,
    List<string>? Movies
);

public record MoviesListUpdateDto(
    string Title,
    List<string>? Movies
);

public record MoviesListResponseDto(
    string Id,
    string Title,
    List<string> Movies,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

