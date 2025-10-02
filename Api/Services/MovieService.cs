using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace cse325_team7_project.Api.Services;

public class MovieService(IMongoCollection<Movie> movies) : IMovieService
{
    private readonly IMongoCollection<Movie> _movies = movies;

    public async Task<IReadOnlyList<Movie>> List()
    {
        var cursor = await _movies.FindAsync(FilterDefinition<Movie>.Empty);
        return await cursor.ToListAsync();
    }

    public async Task<Movie> Get(ObjectId id)
    {
        var movie = await _movies.Find(m => m.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException($"Movie {id} not found");
        return movie;
    }

    public async Task<Movie> Create(Movie movie)
    {
        movie.UpdatedAt = DateTime.UtcNow;
        await _movies.InsertOneAsync(movie);
        return movie;
    }

    public async Task<Movie> Update(ObjectId id, Movie update)
    {
        var current = await _movies.Find(m => m.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException($"Movie {id} not found");
        update.Id = current.Id;
        update.CreatedAt = current.CreatedAt;
        update.UpdatedAt = DateTime.UtcNow;

        await _movies.ReplaceOneAsync(m => m.Id == id, update);
        return update;
    }

    public async Task<bool> Delete(ObjectId id)
    {
        var result = await _movies.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}
