using cse325_team7_project.Domain.Models;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Services.Interfaces;

/// <summary>
/// Defines the movie-specific operations exposed to controllers.
/// </summary>
public interface IMovieService
{
    /// <summary>Returns every movie ordered by the persistence default.</summary>
    Task<IReadOnlyList<Movie>> List();

    /// <summary>Loads a single movie, throwing when it cannot be found.</summary>
    Task<Movie> Get(ObjectId id);

    /// <summary>Persists a new movie document and returns the stored entity.</summary>
    Task<Movie> Create(Movie movie);

    /// <summary>Replaces an existing movie document while keeping creation metadata.</summary>
    Task<Movie> Update(ObjectId id, Movie update);

    /// <summary>Removes a movie by id. Returns <c>true</c> when a document was deleted.</summary>
    Task<bool> Delete(ObjectId id);
}
