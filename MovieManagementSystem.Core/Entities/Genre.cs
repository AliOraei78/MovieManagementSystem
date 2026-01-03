namespace MovieManagementSystem.Core.Entities;

public class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public virtual List<Movie> Movies { get; set; } = new();
}