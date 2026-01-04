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

    // Soft Delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    // Multi-Tenancy
    public int TenantId { get; set; }

    // Navigation property: one studio can have multiple movies
    public List<Movie> Movies { get; set; } = new();
}
