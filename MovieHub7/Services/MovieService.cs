using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub7.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieHub7.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;

        public MovieService(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.DatabaseName);
            _movies = db.GetCollection<Movie>(options.Value.MovieCollectionName);
        }

        public async Task<List<Movie>> GetAsync() =>
            await _movies.Find(_ => true).ToListAsync();

        public async Task<Movie?> GetAsync(string id) =>
            await _movies.Find(m => m.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Movie movie) =>
            await _movies.InsertOneAsync(movie);

        public async Task UpdateAsync(string id, Movie updated) =>
            await _movies.ReplaceOneAsync(m => m.Id == id, updated);

        public async Task RemoveAsync(string id) =>
            await _movies.DeleteOneAsync(m => m.Id == id);

        public async Task SeedAsync()
        {
            var count = await _movies.CountDocumentsAsync(_ => true);
            if (count == 0)
            {
                var sample = new Movie
                {
                    Title = "Inception",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Genre = "Sci-Fi",
                    Description = "A thief who steals corporate secrets through dream-sharing.",
                    Status = "released",
                    Cast = new List<CastType>
                    {
                        new CastType { Role = "Lead", Name = "Leonardo DiCaprio" },
                        new CastType { Role = "Director", Name = "Christopher Nolan" }
                    },
                    Budget = 160000000m
                };

                await _movies.InsertOneAsync(sample);
            }
        }
    }
}
