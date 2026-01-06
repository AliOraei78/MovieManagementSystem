using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Data;
using MovieManagementSystem.Infrastructure.Services; // Added
using System;
using Xunit;

namespace MovieManagementSystem.Tests;

public class MovieRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;

    public MovieRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // 1. Create and configure a fake tenant
        var tenant = new CurrentTenant();
        tenant.SetTenant(1); // TenantId = 1

        // 2. Inject the tenant into the DbContext
        _context = new AppDbContext(options, tenant);

        // Seed data
        _context.Movies.AddRange(
            new Movie
            {
                Id = 1,
                Title = "Inception",
                Rating = 8.8m,
                TenantId = 1, // Must match the tenant set above
                IsDeleted = false,
                RowVersion = new byte[8]
            },
            new Movie
            {
                Id = 2,
                Title = "Matrix",
                Rating = 8.7m,
                TenantId = 1,
                IsDeleted = false,
                RowVersion = new byte[8]
            }
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetHighRatedMovies_ReturnsMoviesAbove8()
    {
        // Act
        var highRated = await _context.Movies
            .Where(m => m.Rating > 8)
            .ToListAsync();

        // Assert
        Assert.Equal(2, highRated.Count);
        Assert.Contains(highRated, m => m.Title == "Inception");
    }

    [Fact]
    public async Task AddMovie_IncreasesCount()
    {
        // Arrange
        var newMovie = new Movie
        {
            Title = "New Movie",
            Rating = 9.0m,
            TenantId = 1, // Important
            IsDeleted = false,
            RowVersion = new byte[8]
        };

        // Act
        _context.Movies.Add(newMovie);
        await _context.SaveChangesAsync();

        // Assert
        Assert.Equal(3, await _context.Movies.CountAsync());
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
