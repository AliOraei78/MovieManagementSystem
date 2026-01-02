using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Core.Entities;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime ReleaseDate { get; set; }

    public decimal Rating { get; set; } = 0;

    public int DurationMinutes { get; set; }

    // One-to-Many: a movie belongs to one studio
    public int StudioId { get; set; }
    public Studio Studio { get; set; } = null!;

    // Many-to-Many: a movie has multiple genres
    public List<Genre> Genres { get; set; } = new();

    // Many-to-Many: a movie has multiple actors
    public List<Actor> Actors { get; set; } = new();

    // One-to-One: a movie has one additional detail
    public MovieDetail MovieDetail { get; set; } = null!;
}
