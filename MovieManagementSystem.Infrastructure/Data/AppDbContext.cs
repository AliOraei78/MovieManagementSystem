using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;

namespace MovieManagementSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial data
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action", Description = "High-energy movies" },
            new Genre { Id = 2, Name = "Drama", Description = "Emotional and character-driven" },
            new Genre { Id = 3, Name = "Comedy", Description = "Humorous films" },
            new Genre { Id = 4, Name = "Sci-Fi", Description = "Science fiction and futuristic" }
        );

        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                Id = 1,
                Title = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                ReleaseDate = new DateTime(2010, 7, 16, 0, 0, 0, DateTimeKind.Utc),
                Rating = 8.8m,
                DurationMinutes = 148
            },
            new Movie
            {
                Id = 2,
                Title = "The Matrix",
                Description = "A computer hacker learns about the true nature of reality.",
                ReleaseDate = new DateTime(1999, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                Rating = 8.7m,
                DurationMinutes = 136
            }
        );
    }
}