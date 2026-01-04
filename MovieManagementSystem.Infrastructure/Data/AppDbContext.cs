using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Services;
using System;

namespace MovieManagementSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly CurrentTenant _currentTenant;
    public AppDbContext(DbContextOptions<AppDbContext> options, CurrentTenant currentTenant)
            : base(options)
    {
        _currentTenant = currentTenant;
    }

    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;
    public DbSet<Studio> Studios { get; set; } = null!;
    public DbSet<MovieDetail> MovieDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Query Filter for Soft Delete
        modelBuilder.Entity<Movie>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Genre>().HasQueryFilter(g => !g.IsDeleted);
        modelBuilder.Entity<Actor>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Studio>().HasQueryFilter(s => !s.IsDeleted);

        // Global Query Filter for Multi-Tenancy
        // We assume the current TenantId is obtained from HttpContext or a service
        modelBuilder.Entity<Movie>().HasQueryFilter(m => m.TenantId == _currentTenant.Id);
        modelBuilder.Entity<Genre>().HasQueryFilter(g => g.TenantId == _currentTenant.Id);
        modelBuilder.Entity<Actor>().HasQueryFilter(a => a.TenantId == _currentTenant.Id);
        modelBuilder.Entity<Studio>().HasQueryFilter(s => s.TenantId == _currentTenant.Id);

        // Movie Configuration
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(m => m.Description)
                  .HasMaxLength(1000);

            entity.Property(m => m.Rating)
                  .HasPrecision(3, 1);  // e.g., 8.7

            entity.Property(m => m.ReleaseDate)
                  .HasColumnType("date");

            // Index on Title for search
            entity.HasIndex(m => m.Title);

            // Composite index for common queries
            entity.HasIndex(m => new { m.ReleaseDate, m.Rating });

            // Concurrency Token (for Optimistic Concurrency)
            entity.Property(m => m.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();

            // Movie → Studio (One-to-Many)
            entity.HasOne(m => m.Studio)
                  .WithMany(s => s.Movies)
                  .HasForeignKey(m => m.StudioId)
                  .OnDelete(DeleteBehavior.SetNull);

            // Movie → Genres (Many-to-Many)
            entity.HasMany(m => m.Genres)
                  .WithMany(g => g.Movies)
                  .UsingEntity(j => j.ToTable("MovieGenres"));

            // Movie → Actors (Many-to-Many)
            entity.HasMany(m => m.Actors)
                  .WithMany(a => a.Movies)
                  .UsingEntity(j => j.ToTable("MovieActors"));

            // Movie → MovieDetail (One-to-One)
            entity.HasOne(m => m.MovieDetail)
                  .WithOne(md => md.Movie)
                  .HasForeignKey<MovieDetail>(md => md.Id);
        });

        // Genre Configuration
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasIndex(g => g.Name)
                  .IsUnique();  // Genre name must be unique
        });

        // Actor Configuration
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(a => a.Name);
        });

        // Studio Configuration
        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.Country)
                  .HasMaxLength(50);

            entity.HasIndex(s => s.Name);
        });

        // MovieDetail Configuration (as Owned Type optional — can be separated later)
        modelBuilder.Entity<MovieDetail>(entity =>
        {
            entity.HasKey(md => md.Id);

            entity.Property(md => md.Language)
                  .HasMaxLength(50);

            entity.Property(md => md.Country)
                  .HasMaxLength(50);

            entity.Property(md => md.Budget)
                  .HasColumnType("decimal(18,2)");

            entity.Property(md => md.Revenue)
                  .HasColumnType("decimal(18,2)");
        });

        // 1. TPH for Person
        modelBuilder.Entity<Person>()
            .UseTphMappingStrategy()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Member>("Member")
            .HasValue<Librarian>("Librarian");

        // 2. TPT for Content
        modelBuilder.Entity<Content>()
            .UseTptMappingStrategy();

        modelBuilder.Entity<Film>()
            .ToTable("Films");

        modelBuilder.Entity<Documentary>()
            .ToTable("Documentaries");

        // 3. TPC for Product
        modelBuilder.Entity<Product>()
            .UseTpcMappingStrategy();

        modelBuilder.Entity<Book>()
            .ToTable("Books");

        modelBuilder.Entity<DigitalBook>()
            .ToTable("DigitalBooks");


        // Updated seed data
        modelBuilder.Entity<Studio>().HasData(
            new Studio
            {
                Id = 1,
                Name = "Warner Bros.",
                Country = "USA",
                FoundedYear = 1923
            },
            new Studio
            {
                Id = 2,
                Name = "Universal Pictures",
                Country = "USA",
                FoundedYear = 1912
            }
        );

        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Drama" },
            new Genre { Id = 3, Name = "Sci-Fi" },
            new Genre { Id = 4, Name = "Comedy" }
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
                StudioId = 1,
                TenantId = 1
            },
            new Movie
            {
                Id = 2,
                Title = "The Matrix",
                Description = "A computer hacker learns about the true nature of reality.",
                ReleaseDate = new DateTime(1999, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                Rating = 8.7m,
                DurationMinutes = 136,
                StudioId = 2,
                TenantId = 2
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

        // Many-to-Many seed (Movie-Genre)
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.HasData(
                new { MoviesId = 1, GenresId = 3 },  // Inception - Sci-Fi
                new { MoviesId = 1, GenresId = 1 },  // Inception - Action
                new { MoviesId = 2, GenresId = 3 },  // Matrix - Sci-Fi
                new { MoviesId = 2, GenresId = 1 }   // Matrix - Action
            ));
    }
}
