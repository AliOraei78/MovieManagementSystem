using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Core.Entities;

public class Studio
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Country { get; set; }

    public int FoundedYear { get; set; }

    // Navigation property: one studio can have multiple movies
    public List<Movie> Movies { get; set; } = new();
}
