using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Data;
using MovieManagementSystem.Infrastructure.Services;
using Xunit;

namespace MovieManagementSystem.Tests;

// A test-specific DbContext to change behavior for SQLite
public class TestAppDbContext : AppDbContext
{
    public TestAppDbContext(DbContextOptions<AppDbContext> options, CurrentTenant currentTenant)
        : base(options, currentTenant) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // In SQLite, make the RowVersion field optional to avoid NOT NULL errors
        modelBuilder.Entity<Movie>()
            .Property(m => m.RowVersion)
            .IsRequired(false);
    }
}

public class MovieSqliteTests : IDisposable
{
    private readonly TestAppDbContext _context;
    private readonly SqliteConnection _connection;

    public MovieSqliteTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        var tenant = new CurrentTenant();
        tenant.SetTenant(1);

        // Use the test-specific DbContext
        _context = new TestAppDbContext(options, tenant);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task SqliteTest_WorksLikeRealDb()
    {
        // 1. First, create and save a studio so it gets an ID
        var studio = new Studio
        {
            Name = "Warner Bros",
            Country = "USA"
        };
        _context.Studios.Add(studio);
        await _context.SaveChangesAsync();
        // This step is critical to avoid Foreign Key errors

        // 2. Now create the movie using a valid StudioId
        var movie = new Movie
        {
            Title = "Sqlite Test Movie",
            TenantId = 1,
            RowVersion = new byte[8],
            ReleaseDate = DateTime.UtcNow,
            StudioId = studio.Id, // Use the studio ID saved in the previous step
            MovieDetail = new MovieDetail // If MovieDetail is required
            {
                Language = "English",
                Country = "USA"
            }
        };

        // 3. Save the movie
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Movies.AnyAsync(m => m.Title == "Sqlite Test Movie");
        Assert.True(result);
    }

    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }
}
