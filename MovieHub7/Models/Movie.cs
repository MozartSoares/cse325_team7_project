namespace MovieHub7.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;

    [Display(Name = "Release Date"), DataType(DataType.Date)]
    public DateTime ReleaseYear { get; set; }

    public Genre Genre { get; set; }

    public string? Description { get; set; }
    public string? Studio { get; set; }

    public string? Image { get; set; }
    public string? ThumbnailImage { get; set; }
    public decimal Budget { get; set; }

    public List<Cast> Casts { get; set; } = new();

}

public enum Genre
{
    Action,
    Comedy,
    Drama,
    Horror,
    SciFi,
    Romance
}