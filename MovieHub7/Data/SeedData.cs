using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieHub7.Models;

namespace MovieHub7.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MovieHub7Context(
                serviceProvider.GetRequiredService<DbContextOptions<MovieHub7Context>>());

            // Look for any movies already in the database
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            context.Movies.AddRange(
                new Movie
                {
                    Title = "Inception",
                    ReleaseYear = new DateTime(2010, 7, 16),
                    Genre = Genre.SciFi,
                    Studio = "Warner Bros.",
                    Budget = 160_000_000M,
                    Description = "A mind-bending thriller about dreams within dreams where Dom Cobb leads a team of specialists to perform extraction and inception.",
                    Image = "img/inception-poster.jpg",
                    ThumbnailImage = "img/inception-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Leonardo DiCaprio", Role = "Dom Cobb" },
                        new Cast { Name = "Joseph Gordon-Levitt", Role = "Arthur" },
                        new Cast { Name = "Marion Cotillard", Role = "Mal" }
                    }
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    ReleaseYear = new DateTime(2008, 7, 18),
                    Genre = Genre.Action,
                    Studio = "Warner Bros.",
                    Budget = 185_000_000M,
                    Description = "Batman faces the Joker in Gotham City in this critically acclaimed superhero masterpiece.",
                    Image = "img/dark-knight-poster.jpg",
                    ThumbnailImage = "img/dark-knight-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Christian Bale", Role = "Bruce Wayne / Batman" },
                        new Cast { Name = "Heath Ledger", Role = "Joker" },
                        new Cast { Name = "Aaron Eckhart", Role = "Harvey Dent" }
                    }
                },
                new Movie
                {
                    Title = "Pulp Fiction",
                    ReleaseYear = new DateTime(1994, 10, 14),
                    Genre = Genre.Drama,
                    Studio = "Miramax Films",
                    Budget = 8_000_000M,
                    Description = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption.",
                    Image = "img/pulp-fiction-poster.jpg",
                    ThumbnailImage = "img/pulp-fiction-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "John Travolta", Role = "Vincent Vega" },
                        new Cast { Name = "Samuel L. Jackson", Role = "Jules Winnfield" },
                        new Cast { Name = "Uma Thurman", Role = "Mia Wallace" }
                    }
                },
                new Movie
                {
                    Title = "The Shawshank Redemption",
                    ReleaseYear = new DateTime(1994, 9, 23),
                    Genre = Genre.Drama,
                    Studio = "Columbia Pictures",
                    Budget = 25_000_000M,
                    Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    Image = "img/shawshank-poster.jpg",
                    ThumbnailImage = "img/shawshank-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Tim Robbins", Role = "Andy Dufresne" },
                        new Cast { Name = "Morgan Freeman", Role = "Ellis Boyd 'Red' Redding" },
                        new Cast { Name = "Bob Gunton", Role = "Warden Norton" }
                    }
                },
                new Movie
                {
                    Title = "Forrest Gump",
                    ReleaseYear = new DateTime(1994, 7, 6),
                    Genre = Genre.Drama,
                    Studio = "Paramount Pictures",
                    Budget = 55_000_000M,
                    Description = "The presidencies of Kennedy and Johnson, Vietnam, Watergate, and other history unfold through the perspective of an Alabama man.",
                    Image = "img/forrest-gump-poster.jpg",
                    ThumbnailImage = "img/forrest-gump-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Tom Hanks", Role = "Forrest Gump" },
                        new Cast { Name = "Robin Wright", Role = "Jenny Curran" },
                        new Cast { Name = "Gary Sinise", Role = "Lieutenant Dan Taylor" }
                    }
                },
                new Movie
                {
                    Title = "The Matrix",
                    ReleaseYear = new DateTime(1999, 3, 31),
                    Genre = Genre.SciFi,
                    Studio = "Warner Bros.",
                    Budget = 63_000_000M,
                    Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    Image = "img/matrix-poster.jpg",
                    ThumbnailImage = "img/matrix-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Keanu Reeves", Role = "Neo" },
                        new Cast { Name = "Laurence Fishburne", Role = "Morpheus" },
                        new Cast { Name = "Carrie-Anne Moss", Role = "Trinity" }
                    }
                },
                new Movie
                {
                    Title = "Goodfellas",
                    ReleaseYear = new DateTime(1990, 9, 21),
                    Genre = Genre.Drama,
                    Studio = "Warner Bros.",
                    Budget = 25_000_000M,
                    Description = "The story of Henry Hill and his life in the mob, covering his relationship with his wife Karen Hill and his mob partners.",
                    Image = "img/goodfellas-poster.jpg",
                    ThumbnailImage = "img/goodfellas-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Robert De Niro", Role = "James Conway" },
                        new Cast { Name = "Ray Liotta", Role = "Henry Hill" },
                        new Cast { Name = "Joe Pesci", Role = "Tommy DeVito" }
                    }
                },
                new Movie
                {
                    Title = "The Godfather",
                    ReleaseYear = new DateTime(1972, 3, 24),
                    Genre = Genre.Drama,
                    Studio = "Paramount Pictures",
                    Budget = 6_000_000M,
                    Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    Image = "img/godfather-poster.jpg",
                    ThumbnailImage = "img/godfather-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Marlon Brando", Role = "Vito Corleone" },
                        new Cast { Name = "Al Pacino", Role = "Michael Corleone" },
                        new Cast { Name = "James Caan", Role = "Sonny Corleone" }
                    }
                },
                new Movie
                {
                    Title = "Titanic",
                    ReleaseYear = new DateTime(1997, 12, 19),
                    Genre = Genre.Romance,
                    Studio = "Paramount Pictures",
                    Budget = 200_000_000M,
                    Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.",
                    Image = "img/titanic-poster.jpg",
                    ThumbnailImage = "img/titanic-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Leonardo DiCaprio", Role = "Jack Dawson" },
                        new Cast { Name = "Kate Winslet", Role = "Rose DeWitt Bukater" },
                        new Cast { Name = "Billy Zane", Role = "Caledon Hockley" }
                    }
                },
                new Movie
                {
                    Title = "Avatar",
                    ReleaseYear = new DateTime(2009, 12, 18),
                    Genre = Genre.SciFi,
                    Studio = "20th Century Fox",
                    Budget = 237_000_000M,
                    Description = "A paraplegic Marine dispatched to the moon Pandora on a unique mission becomes torn between following his orders and protecting the world he feels is his home.",
                    Image = "img/avatar-poster.jpg",
                    ThumbnailImage = "img/avatar-thumb.jpg",
                    Casts = new List<Cast>
                    {
                        new Cast { Name = "Sam Worthington", Role = "Jake Sully" },
                        new Cast { Name = "Zoe Saldana", Role = "Neytiri" },
                        new Cast { Name = "Sigourney Weaver", Role = "Dr. Grace Augustine" }
                    }
                }
            );

            context.SaveChanges();
        }

    }
}
