using Microsoft.EntityFrameworkCore;
using MovieHub7.Models;

namespace MovieHub7.Data
{
    public class MovieHub7Context : DbContext
    {
        public MovieHub7Context(DbContextOptions<MovieHub7Context> options)
            : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cast> Casts { get; set; }
    }
}
