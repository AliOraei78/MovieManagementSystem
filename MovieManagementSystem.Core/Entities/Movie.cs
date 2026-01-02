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

    // Navigation properties will be added later
}