using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace cse325_team7_project.Api.Services;

/// <summary>
/// MongoDB implementation of <see cref="IMoviesListService"/>.
/// Validates referenced movie ids to keep list documents consistent.
/// </summary>
public class MoviesListService(IMongoCollection<MoviesList> lists, IMongoCollection<Movie> movies, IMongoCollection<User> users) : IMoviesListService
{
    private readonly IMongoCollection<MoviesList> _lists = lists;
    private readonly IMongoCollection<Movie> _movies = movies;
    private readonly IMongoCollection<User> _users = users;
    /// <inheritdoc />
    public async Task<IReadOnlyList<MoviesList>> List()
    {
        var cursor = await _lists.FindAsync(FilterDefinition<MoviesList>.Empty);
        return await cursor.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<MoviesList> Get(ObjectId id)
    {
        var list = await _lists.Find(l => l.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException($"List {id} not found");
        return list;
    }

    /// <inheritdoc />
    public async Task<MoviesList> Create(MoviesList list,ObjectId ownerId)
    {
        await ValidateMovieIds(list.Movies);
        list.UpdatedAt = DateTime.UtcNow;
        await _lists.InsertOneAsync(list);
        var owner = await _users.Find(u => u.Id == ownerId).FirstOrDefaultAsync() ?? throw new NotFoundException($"Owner {ownerId} not found");
        owner.Lists.Add(list.Id);
        owner.UpdatedAt = DateTime.UtcNow;
        await _users.ReplaceOneAsync(u => u.Id == ownerId, owner);
        return list;
    }

    /// <inheritdoc />
    public async Task<MoviesList> Update(ObjectId id, MoviesList update)
    {
        var current = await _lists.Find(l => l.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException($"List {id} not found");

        await ValidateMovieIds(update.Movies);

        update.Id = current.Id;
        update.CreatedAt = current.CreatedAt;
        update.UpdatedAt = DateTime.UtcNow;

        await _lists.ReplaceOneAsync(l => l.Id == id, update);
        return update;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(ObjectId id)
    {
        var result = await _lists.DeleteOneAsync(l => l.Id == id);
        if (result.DeletedCount == 0)
        {
            return false;
        }

        var filter = Builders<User>.Filter.AnyEq(u => u.Lists, id);
        var update = Builders<User>.Update.Pull(u => u.Lists, id).Set(u => u.UpdatedAt, DateTime.UtcNow);
        await _users.UpdateManyAsync(filter, update);

        return true;
    }

    /// <summary>
    /// Ensures every referenced movie exists before writing the list document.
    /// </summary>
    private async Task ValidateMovieIds(IReadOnlyCollection<ObjectId> movieIds)
    {
        if (movieIds is null || movieIds.Count == 0) return;
        var filter = Builders<Movie>.Filter.In(m => m.Id, movieIds);
        var count = await _movies.CountDocumentsAsync(filter);
        if (count != movieIds.Count)
        {
            throw new ValidationException("One or more movie ids are invalid");
        }
    }
}
