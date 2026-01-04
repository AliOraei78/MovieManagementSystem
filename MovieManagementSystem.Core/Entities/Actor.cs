using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Core.Entities;

public class Actor
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public string? Biography { get; set; }

    // Navigation property
    public List<Movie> Movies { get; set; } = new();
}