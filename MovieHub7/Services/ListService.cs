using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub7.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieHub7.Services
{
    public class ListService
    {
        private readonly IMongoCollection<ListModel> _lists;

        public ListService(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.DatabaseName);
            _lists = db.GetCollection<ListModel>(options.Value.ListCollectionName);
        }

        public async Task<List<ListModel>> GetAsync() =>
            await _lists.Find(_ => true).ToListAsync();

        public async Task<ListModel?> GetAsync(string id) =>
            await _lists.Find(l => l.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ListModel list) =>
            await _lists.InsertOneAsync(list);

        public async Task UpdateAsync(string id, ListModel updated) =>
            await _lists.ReplaceOneAsync(l => l.Id == id, updated);

        public async Task RemoveAsync(string id) =>
            await _lists.DeleteOneAsync(l => l.Id == id);

        public async Task SeedAsync()
        {
            var count = await _lists.CountDocumentsAsync(_ => true);
            if (count == 0)
            {
                var sample = new ListModel
                {
                    Title = "Sample Movie List",
                    Movies = new List<string> { "sampleMovieId1", "sampleMovieId2" },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _lists.InsertOneAsync(sample);
            }
        }
    }
}