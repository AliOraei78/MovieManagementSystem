namespace MovieManagementSystem.Core.Entities;

public class MovieDetail
{
    public int Id { get; set; }  // Same as the Movie Id

    public string? Language { get; set; }

    public string? Country { get; set; }

    public decimal Budget { get; set; }

    public decimal Revenue { get; set; }

    // Navigation property
    public Movie Movie { get; set; } = null!;
}
