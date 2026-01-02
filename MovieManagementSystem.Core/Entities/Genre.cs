using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Core.Entities;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Navigation property
    public List<Movie> Movies { get; set; } = new();
}