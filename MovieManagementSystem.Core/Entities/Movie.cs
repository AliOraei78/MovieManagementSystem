using MovieManagementSystem.Core.Interfaces;

namespace MovieManagementSystem.Core.Entities;

public class Movie : ISoftDelete, IHasTenant
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime ReleaseDate { get; set; }

    public decimal Rating { get; set; }

    public int DurationMinutes { get; set; }

    // Soft Delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    // Multi-Tenancy
    public int TenantId { get; set; }

    public int StudioId { get; set; }
    public Studio Studio { get; set; } = null!;

    public List<Genre> Genres { get; set; } = new();
    public List<Actor> Actors { get; set; } = new();

    public MovieDetail MovieDetail { get; set; } = null!;

    public byte[] RowVersion { get; set; } = null!;
}