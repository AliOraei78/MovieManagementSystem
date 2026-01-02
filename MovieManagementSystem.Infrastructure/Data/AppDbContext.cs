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
    public DbSet<Studio> Studios { get; set; } = null!;
    public DbSet<MovieDetail> MovieDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One-to-Many: Movie -> Studio
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Studio)
            .WithMany(s => s.Movies)
            .HasForeignKey(m => m.StudioId)
            .OnDelete(DeleteBehavior.SetNull);
        // If a studio is deleted, set StudioId to null

        // Many-to-Many: Movie - Genre (automatic join table)
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MovieGenres"));

        // Many-to-Many: Movie - Actor
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            .UsingEntity(j => j.ToTable("MovieActors"));

        // One-to-One: Movie - MovieDetail (shared primary key)
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.MovieDetail)
            .WithOne(md => md.Movie)
            .HasForeignKey<MovieDetail>(md => md.Id);

        // Updated seed data
        modelBuilder.Entity<Studio>().HasData(
            new Studio { Id = 1, Name = "Warner Bros.", Country = "USA", FoundedYear = 1923 },
            new Studio { Id = 2, Name = "Universal Pictures", Country = "USA", FoundedYear = 1912 }
        );

        // Updated Movie seed with relationships
        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                Id = 1,
                Title = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                ReleaseDate = new DateTime(2010, 7, 16, 0, 0, 0, DateTimeKind.Utc),
                Rating = 8.8m,
                DurationMinutes = 148,
                StudioId = 1
            },
            new Movie
            {
                Id = 2,
                Title = "The Matrix",
                Description = "A computer hacker learns about the true nature of reality.",
                ReleaseDate = new DateTime(1999, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                Rating = 8.7m,
                DurationMinutes = 136,
                StudioId = 2
            }
        );

        modelBuilder.Entity<MovieDetail>().HasData(
            new MovieDetail
            {
                Id = 1,  // Same as Movie Id
                Language = "English",
                Country = "USA",
                Budget = 160000000m,
                Revenue = 836800000m
            },
            new MovieDetail
            {
                Id = 2,
                Language = "English",
                Country = "USA",
                Budget = 63000000m,
                Revenue = 463500000m
            }
        );
    }
}
