using cse325_team7_project.Domain.Models;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Services.Interfaces;

public interface IMoviesListService
{
    Task<IReadOnlyList<MoviesList>> List();
    Task<MoviesList> Get(ObjectId id);
    Task<MoviesList> Create(MoviesList list);
    Task<MoviesList> Update(ObjectId id, MoviesList update);
    Task<bool> Delete(ObjectId id);
}
