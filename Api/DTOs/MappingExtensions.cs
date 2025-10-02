using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Domain.Models;
using cse325_team7_project.Domain.ValueObjects;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Mappings;

/// <summary>
/// Centralizes conversions between API DTOs and domain models so controllers stay focused on orchestration.
/// </summary>
public static class MappingExtensions
{
    // Movie
    public static MovieResponseDto ToDto(this Movie m) => new(
        m.Id.ToString(),
        m.Title,
        m.ReleaseDate,
        m.Genre,
        m.Description,
        m.Studio,
        new List<CastMember>(m.Cast),
        m.Image,
        m.ThumbnailImage,
        m.Budget,
        m.CreatedAt,
        m.UpdatedAt
    );

    public static Movie ToModel(this MovieCreateDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            Genre = dto.Genre,
            Description = dto.Description,
            Studio = dto.Studio,
            Cast = dto.Cast?.Select(c => new CastMember { Name = c.Name, Role = c.Role }).ToList() ?? [],
            Image = dto.Image,
            ThumbnailImage = dto.ThumbnailImage,
            Budget = dto.Budget
        };
        return movie;
    }

    public static void Apply(this Movie target, MovieUpdateDto dto)
    {
        target.Title = dto.Title;
        target.ReleaseDate = dto.ReleaseDate;
        target.Genre = dto.Genre;
        target.Description = dto.Description;
        target.Studio = dto.Studio;
        target.Cast = dto.Cast?.Select(c => new CastMember { Name = c.Name, Role = c.Role }).ToList() ?? [];
        target.Image = dto.Image;
        target.ThumbnailImage = dto.ThumbnailImage;
        target.Budget = dto.Budget;
    }

    // User
    public static UserResponseDto ToDto(this User u) => new(
        u.Id.ToString(),
        u.Username,
        u.Name,
        u.Email,
        u.Lists.Select(x => x.ToString()).ToList(),
        u.Role,
        u.CreatedAt,
        u.UpdatedAt
    );

    public static User ToModel(this UserCreateDto dto)
    {
        return new User
        {
            Username = dto.Username,
            Name = dto.Name,
            PasswordHash = dto.PasswordHash,
            Email = dto.Email,
            Lists = dto.Lists?.Where(s => ObjectId.TryParse(s, out _)).Select(ObjectId.Parse).ToList() ?? [],
            Role = dto.Role
        };
    }

    public static void Apply(this User target, UserUpdateDto dto)
    {
        target.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
        {
            target.PasswordHash = dto.PasswordHash!;
        }
        target.Email = dto.Email;
        target.Lists = dto.Lists?.Where(s => ObjectId.TryParse(s, out _)).Select(ObjectId.Parse).ToList() ?? target.Lists;
        target.Role = dto.Role;
    }

    // MoviesList
    public static MoviesListResponseDto ToDto(this MoviesList l) => new(
        l.Id.ToString(),
        l.Title,
        l.Movies.Select(x => x.ToString()).ToList(),
        l.CreatedAt,
        l.UpdatedAt
    );

    public static MoviesList ToModel(this MoviesListCreateDto dto)
    {
        return new MoviesList
        {
            Title = dto.Title,
            Movies = dto.Movies?.Where(s => ObjectId.TryParse(s, out _)).Select(ObjectId.Parse).ToList() ?? []
        };
    }

    public static void Apply(this MoviesList target, MoviesListUpdateDto dto)
    {
        target.Title = dto.Title;
        target.Movies = dto.Movies?.Where(s => ObjectId.TryParse(s, out _)).Select(ObjectId.Parse).ToList() ?? target.Movies;
    }
}
