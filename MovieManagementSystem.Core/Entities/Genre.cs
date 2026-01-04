namespace MovieManagementSystem.Core.Entities;

public class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Soft Delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    // Multi-Tenancy
    public int TenantId { get; set; }
    public List<Movie> Movies { get; set; } = new();
}