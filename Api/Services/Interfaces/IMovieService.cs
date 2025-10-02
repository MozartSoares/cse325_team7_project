using cse325_team7_project.Domain.Models;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Services.Interfaces;

public interface IMovieService
{
    Task<IReadOnlyList<Movie>> List();
    Task<Movie> Get(ObjectId id);
    Task<Movie> Create(Movie movie);
    Task<Movie> Update(ObjectId id, Movie update);
    Task<bool> Delete(ObjectId id);
}
