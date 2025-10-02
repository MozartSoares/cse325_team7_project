using cse325_team7_project.Domain.Models;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Services.Interfaces;

/// <summary>
/// Contract for managing curated movie lists.
/// </summary>
public interface IMoviesListService
{
    /// <summary>Returns all lists.</summary>
    Task<IReadOnlyList<MoviesList>> List();

    /// <summary>Fetches a list by id or throws when it does not exist.</summary>
    Task<MoviesList> Get(ObjectId id);

    /// <summary>Creates a new list after validating referenced movies.</summary>
    Task<MoviesList> Create(MoviesList list);

    /// <summary>Replaces an existing list after validating movie references.</summary>
    Task<MoviesList> Update(ObjectId id, MoviesList update);

    /// <summary>Deletes a list and reports whether a document was removed.</summary>
    Task<bool> Delete(ObjectId id);
}
