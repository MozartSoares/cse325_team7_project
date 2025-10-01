using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub7.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieHub7.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.DatabaseName);
            _users = db.GetCollection<User>(options.Value.UserCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateAsync(string id, User updated) =>
            await _users.ReplaceOneAsync(u => u.Id == id, updated);

        public async Task RemoveAsync(string id) =>
            await _users.DeleteOneAsync(u => u.Id == id);

        public async Task SeedAsync()
        {
            var count = await _users.CountDocumentsAsync(_ => true);
            if (count == 0)
            {
                var sample = new User
                {
                    Username = "admin",
                    Name = "Admin User",
                    Password = "hashedpassword123", // Substitua por hash seguro em produção
                    Email = "admin@example.com",
                    Role = UserRole.ADMIN,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _users.InsertOneAsync(sample);
            }
        }
    }
}