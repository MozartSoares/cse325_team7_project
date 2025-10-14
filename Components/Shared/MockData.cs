using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Domain.ValueObjects;

public static class MockData
{
    public static UserResponseDto GetPlaceholderUser()
    {
        return new UserResponseDto(
            Id: "123456",
            Username: "Johndoe",
            Name: "John Doe",
            Email: "mockuser@example.com",
            Lists: new List<string> { "List1" },
            Role: UserRole.User,
            CreatedAt: DateTime.Now.AddDays(-30),
            UpdatedAt: DateTime.Now
        );
    }

    public static List<MovieResponseDto> GetPlaceholderMovies()
    {
        return [
        new MovieResponseDto(
            Id: "one",
            Title: "Play Dirty",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.Crime,
            Description: "Expert thief Parker gets a shot at a major heist...",
            Studio: "Team Downey",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Mark Wahlberg"},
            new CastMember { Role = CastRole.Actor, Name = "LaKeith Stanfield"},
            new CastMember { Role = CastRole.Actor, Name = "Rosa Salazar"},
            new CastMember { Role = CastRole.Actor, Name = "Keegan-Michael Key"},
            new CastMember { Role = CastRole.Actor, Name = "Claire Lovering"},
            new CastMember { Role = CastRole.Director, Name = "Shane Black"},
            new CastMember { Role = CastRole.Producer, Name = "Susan Downey"},
            new CastMember { Role = CastRole.Producer, Name = "Jules Daly"},
            new CastMember { Role = CastRole.Producer, Name = "Marc Toberoff"},
            new CastMember { Role = CastRole.Editor, Name = "Chris Lebenzon"},
            new CastMember { Role = CastRole.Editor, Name = "Joel Negron"},
            },
            Image: "https://image.tmdb.org/t/p/w500/ovZ0zq0NwRghtWI1oLaM0lWuoEw.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/ovZ0zq0NwRghtWI1oLaM0lWuoEw.jpg",
            Budget: 2500000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),
        new MovieResponseDto(
            Id: "two",
            Title: "Demon Slayer: Kimetsu no Yaiba Infinity Castle",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.Animation,
            Description: "The Demon Slayer Corps are drawn into the Infinity Castle...",
            Studio: "ufotable",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Natsuki Hanae"},
            new CastMember { Role = CastRole.Actor, Name = "Hiro Shimono"},
            new CastMember { Role = CastRole.Actor, Name = "Takahiro Sakurai"},
            new CastMember { Role = CastRole.Actor, Name = "Akira Ishida"},
            new CastMember { Role = CastRole.Actor, Name = "Yoshitsugu Matsuoka"},
            new CastMember { Role = CastRole.Director, Name = "Haruo Sotozaki"},
            new CastMember { Role = CastRole.Producer, Name = "Masanori Miyake"},
            new CastMember { Role = CastRole.Producer, Name = "Yuma Takahashi"},
            new CastMember { Role = CastRole.Producer, Name = "Tatsuro Hayashi"},
            new CastMember { Role = CastRole.Producer, Name = "Takao Shimazaki"},
            new CastMember { Role = CastRole.Editor, Name = "Manabu Kamino"},
            },
            Image: "https://image.tmdb.org/t/p/w500/sUsVimPdA1l162FvdBIlmKBlWHx.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/sUsVimPdA1l162FvdBIlmKBlWHx.jpg",
            Budget: 20000000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),
        new MovieResponseDto(
            Id: "three",
            Title: "The Fantastic 4: First Steps",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.ScienceFiction,
            Description: "Against the vibrant backdrop of a 1960s-inspired, retro-futuristic world...",
            Studio: "Marvel Studios",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Pedro Pascal"},
            new CastMember { Role = CastRole.Actor, Name = "Vanessa Kirby"},
            new CastMember { Role = CastRole.Actor, Name = "Ebon Moss-Bachrach"},
            new CastMember { Role = CastRole.Actor, Name = "Joseph Quinn"},
            new CastMember { Role = CastRole.Actor, Name = "Ralph Ineson"},
            new CastMember { Role = CastRole.Director, Name = "Matt Shakman"},
            new CastMember { Role = CastRole.Producer, Name = "Kevin Feige"},
            new CastMember { Role = CastRole.Editor, Name = "Nona Khodai"},
            new CastMember { Role = CastRole.Editor, Name = "Tim Roche"},
            },
            Image: "https://image.tmdb.org/t/p/w500/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            Budget: 200000000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),
        new MovieResponseDto(
            Id: "four",
            Title: "The Fantastic 4: First Steps",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.ScienceFiction,
            Description: "Against the vibrant backdrop of a 1960s-inspired, retro-futuristic world...",
            Studio: "Marvel Studios",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Pedro Pascal"},
            new CastMember { Role = CastRole.Actor, Name = "Vanessa Kirby"},
            new CastMember { Role = CastRole.Actor, Name = "Ebon Moss-Bachrach"},
            new CastMember { Role = CastRole.Actor, Name = "Joseph Quinn"},
            new CastMember { Role = CastRole.Actor, Name = "Ralph Ineson"},
            new CastMember { Role = CastRole.Director, Name = "Matt Shakman"},
            new CastMember { Role = CastRole.Producer, Name = "Kevin Feige"},
            new CastMember { Role = CastRole.Editor, Name = "Nona Khodai"},
            new CastMember { Role = CastRole.Editor, Name = "Tim Roche"},
            },
            Image: "https://image.tmdb.org/t/p/w500/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            Budget: 200000000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),
        new MovieResponseDto(
            Id: "five",
            Title: "The Fantastic 4: First Steps",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.ScienceFiction,
            Description: "Against the vibrant backdrop of a 1960s-inspired, retro-futuristic world...",
            Studio: "Marvel Studios",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Pedro Pascal"},
            new CastMember { Role = CastRole.Actor, Name = "Vanessa Kirby"},
            new CastMember { Role = CastRole.Actor, Name = "Ebon Moss-Bachrach"},
            new CastMember { Role = CastRole.Actor, Name = "Joseph Quinn"},
            new CastMember { Role = CastRole.Actor, Name = "Ralph Ineson"},
            new CastMember { Role = CastRole.Director, Name = "Matt Shakman"},
            new CastMember { Role = CastRole.Producer, Name = "Kevin Feige"},
            new CastMember { Role = CastRole.Editor, Name = "Nona Khodai"},
            new CastMember { Role = CastRole.Editor, Name = "Tim Roche"},
            },
            Image: "https://image.tmdb.org/t/p/w500/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            Budget: 200000000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),
        new MovieResponseDto(
            Id: "six",
            Title: "The Fantastic 4: First Steps",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.ScienceFiction,
            Description: "Against the vibrant backdrop of a 1960s-inspired, retro-futuristic world...",
            Studio: "Marvel Studios",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Pedro Pascal"},
            new CastMember { Role = CastRole.Actor, Name = "Vanessa Kirby"},
            new CastMember { Role = CastRole.Actor, Name = "Ebon Moss-Bachrach"},
            new CastMember { Role = CastRole.Actor, Name = "Joseph Quinn"},
            new CastMember { Role = CastRole.Actor, Name = "Ralph Ineson"},
            new CastMember { Role = CastRole.Director, Name = "Matt Shakman"},
            new CastMember { Role = CastRole.Producer, Name = "Kevin Feige"},
            new CastMember { Role = CastRole.Editor, Name = "Nona Khodai"},
            new CastMember { Role = CastRole.Editor, Name = "Tim Roche"},
            },
            Image: "https://image.tmdb.org/t/p/w500/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            Budget: 200000000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),

        ];
    }

    public static MoviesListResponseDto GetPlaceholderList()
    {
        return new MoviesListResponseDto(
        Id: "one",
        Title: "My Favorite Movies",
        Movies: new List<string> { "one", "two", "three", "four", "five", "six" },
        CreatedAt: DateTime.UtcNow,
        UpdatedAt: DateTime.UtcNow
        );
    }

    public static  List<MovieResponseDto> GetPlaceholderMovies2()
    {
        return new List<MovieResponseDto>
        {
        new MovieResponseDto(
            Id: "1",
            Title: "The Fantastic 4: First Steps",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Genre: Genre.ScienceFiction,
            Description: "Against the vibrant backdrop of a 1960s-inspired, retro-futuristic world, Marvel's First Family is forced to balance their roles as heroes with the strength of their family bond, while defending Earth from a ravenous space god called Galactus and his enigmatic Herald, Silver Surfer.",
            Studio: "Marvel Studios",
            Cast: new List<CastMember>
            {
            new CastMember { Role = CastRole.Actor, Name = "Pedro Pascal"},
            new CastMember { Role = CastRole.Actor, Name = "Vanessa Kirby"},
            new CastMember { Role = CastRole.Actor, Name = "Ebon Moss-Bachrach"},
            new CastMember { Role = CastRole.Actor, Name = "Joseph Quinn"},
            new CastMember { Role = CastRole.Actor, Name = "Ralph Ineson"},
            new CastMember { Role = CastRole.Director, Name = "Matt Shakman"},
            new CastMember { Role = CastRole.Producer, Name = "Kevin Feige"},
            new CastMember { Role = CastRole.Editor, Name = "Nona Khodai"},
            new CastMember { Role = CastRole.Editor, Name = "Tim Roche"},
            },
            Image: "https://image.tmdb.org/t/p/w500/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            ThumbnailImage: "https://image.tmdb.org/t/p/w200/cm8TNGBGG0aBfWj0LgrESHv8tir.jpg",
            Budget: 200000000m,
            CreatedAt: DateTime.Now,
            UpdatedAt: DateTime.Now
            ),
            new MovieResponseDto(
                "2", "Inception", new DateOnly(2010, 7, 16), Genre.ScienceFiction,
                "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea.",
                "Warner Bros", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/cc6600/ffffff?text=Inception",
                "https://via.placeholder.com/200x300/cc6600/ffffff?text=Inception",
                160000000m, DateTime.UtcNow.AddDays(-25), DateTime.UtcNow.AddDays(-25)
            ),
            new MovieResponseDto(
                "3", "The Dark Knight", new DateOnly(2008, 7, 18), Genre.Action,
                "Batman faces the Joker, a criminal mastermind who wants to plunge Gotham City into anarchy.",
                "Warner Bros", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/333333/ffffff?text=Dark+Knight",
                "https://via.placeholder.com/200x300/333333/ffffff?text=Batman",
                185000000m, DateTime.UtcNow.AddDays(-20), DateTime.UtcNow.AddDays(-20)
            ),
            new MovieResponseDto(
                "4", "Pulp Fiction", new DateOnly(1994, 10, 14), Genre.Crime,
                "The lives of two mob hitmen, a boxer, and others intertwine in four tales of violence and redemption.",
                "Miramax", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/990000/ffffff?text=Pulp+Fiction",
                "https://via.placeholder.com/200x300/990000/ffffff?text=Pulp",
                8500000m, DateTime.UtcNow.AddDays(-15), DateTime.UtcNow.AddDays(-15)
            ),
            new MovieResponseDto(
                "5", "The Shawshank Redemption", new DateOnly(1994, 9, 23), Genre.Drama,
                "Two imprisoned men bond over years, finding solace and eventual redemption through acts of common decency.",
                "Columbia Pictures", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/006600/ffffff?text=Shawshank",
                "https://via.placeholder.com/200x300/006600/ffffff?text=Shawshank",
                25000000m, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10)
            ),
            new MovieResponseDto(
                "6", "Forrest Gump", new DateOnly(1994, 7, 6), Genre.Drama,
                "The presidencies of Kennedy and Johnson through the eyes of an Alabama man with an IQ of 75.",
                "Paramount Pictures", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/0099cc/ffffff?text=Forrest+Gump",
                "https://via.placeholder.com/200x300/0099cc/ffffff?text=Forrest",
                55000000m, DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(-5)
            ),
            new MovieResponseDto(
                "7", "Avatar", new DateOnly(2009, 12, 18), Genre.ScienceFiction,
                "A paraplegic Marine dispatched to the moon Pandora on a unique mission becomes torn between following orders and protecting an alien civilization.",
                "20th Century Fox", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/0066ff/ffffff?text=Avatar",
                "https://via.placeholder.com/200x300/0066ff/ffffff?text=Avatar",
                237000000m, DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddDays(-3)
            ),
            new MovieResponseDto(
                "8", "Titanic", new DateOnly(1997, 12, 19), Genre.Romance,
                "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.",
                "Paramount Pictures", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/003366/ffffff?text=Titanic",
                "https://via.placeholder.com/200x300/003366/ffffff?text=Titanic",
                200000000m, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-2)
            ),
            new MovieResponseDto(
                "9", "The Lion King", new DateOnly(1994, 6, 24), Genre.Animation,
                "A young lion prince flees his kingdom only to learn the true meaning of responsibility and bravery.",
                "Walt Disney Pictures", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/ff9900/ffffff?text=Lion+King",
                "https://via.placeholder.com/200x300/ff9900/ffffff?text=Lion",
                45000000m, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(-1)
            ),
            new MovieResponseDto(
                "10", "Jurassic Park", new DateOnly(1993, 6, 11), Genre.Adventure,
                "A pragmatic paleontologist visiting an almost complete theme park is tasked with protecting a couple of kids after a power failure causes the park's cloned dinosaurs to run loose.",
                "Universal Pictures", new List<cse325_team7_project.Domain.ValueObjects.CastMember>(),
                "https://via.placeholder.com/400x600/669900/ffffff?text=Jurassic+Park",
                "https://via.placeholder.com/200x300/669900/ffffff?text=Jurassic",
                63000000m, DateTime.UtcNow, DateTime.UtcNow
            )
        };
    }

    public static List<MoviesListResponseDto> GetPlaceholderLists()
    {
        return [

            new MoviesListResponseDto(
            Id: "one",
            Title: "My Favorite Movies",
            Movies: new List<string> { "one", "two", "three", "four", "five", "six" },
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: DateTime.UtcNow
            ),
            new MoviesListResponseDto(
            Id: "two",
            Title: "Watch later",
            Movies: new List<string> {"four", "five", "six" },
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: DateTime.UtcNow
            ),

        ];
    }
}
