using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace cse325_team7_project.Api.Services;

public class UserService(IMongoCollection<User> users) : IUserService
{
    private readonly IMongoCollection<User> _users = users;

    public async Task<IReadOnlyList<User>> List()
    {
        var cursor = await _users.FindAsync(FilterDefinition<User>.Empty);
        return await cursor.ToListAsync();
    }

    public async Task<User> Get(ObjectId id)
    {
        var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException($"User {id} not found");
        return user;
    }

    public async Task<User> Create(User user)
    {
        // uniqueness via query (in addition to the unique index that the infra will create)
        var existsUsername = await _users.Find(u => u.Username.Equals(user.Username, StringComparison.CurrentCultureIgnoreCase)).AnyAsync();
        if (existsUsername) throw new ConflictException($"Username '{user.Username}' already exists");
        
        var existsEmail = await _users.Find(u => u.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase)).AnyAsync();
        if (existsEmail) throw new ConflictException($"Email '{user.Email}' already exists");

        user.UpdatedAt = DateTime.UtcNow;
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> Update(ObjectId id, User update)
    {
        var current = await _users.Find(u => u.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException($"User {id} not found");
        var usernameChanged = !string.Equals(current.Username, update.Username, StringComparison.OrdinalIgnoreCase);
        var emailChanged = !string.Equals(current.Email, update.Email, StringComparison.OrdinalIgnoreCase);

        if (usernameChanged)
        {
            var existsUsername = await _users.Find(u => u.Username.Equals(update.Username, StringComparison.CurrentCultureIgnoreCase) && u.Id != id).AnyAsync();
            if (existsUsername) throw new ConflictException($"Username '{update.Username}' already exists");
        }
        if (emailChanged)
        {
            var existsEmail = await _users.Find(u => u.Email.Equals(update.Email, StringComparison.CurrentCultureIgnoreCase) && u.Id != id).AnyAsync();
            if (existsEmail) throw new ConflictException($"Email '{update.Email}' already exists");
        }

        update.Id = current.Id;
        update.CreatedAt = current.CreatedAt;
        update.UpdatedAt = DateTime.UtcNow;

        await _users.ReplaceOneAsync(u => u.Id == id, update);
        return update;
    }

    public async Task<bool> Delete(ObjectId id)
    {
        var result = await _users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }
}
