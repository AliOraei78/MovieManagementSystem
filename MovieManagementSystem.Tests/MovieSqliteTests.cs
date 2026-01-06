using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Data;
using MovieManagementSystem.Infrastructure.Services;
using Xunit;

namespace MovieManagementSystem.Tests;

// یک کانتکست مخصوص تست برای تغییر رفتار در SQLite
public class TestAppDbContext : AppDbContext
{
    public TestAppDbContext(DbContextOptions<AppDbContext> options, CurrentTenant currentTenant)
        : base(options, currentTenant) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // در SQLite فیلد RowVersion را اختیاری می‌کنیم تا خطای NOT NULL ندهد
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

        // استفاده از کانتکست مخصوص تست
        _context = new TestAppDbContext(options, tenant);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task SqliteTest_WorksLikeRealDb()
    {
        // 1. ابتدا یک استودیو بسازید و ذخیره کنید تا ID بگیرد
        var studio = new Studio
        {
            Name = "Warner Bros",
            Country = "USA"
        };
        _context.Studios.Add(studio);
        await _context.SaveChangesAsync(); // این مرحله برای رفع خطای Foreign Key حیاتی است

        // 2. حالا فیلم را با استفاده از StudioId معتبر بسازید
        var movie = new Movie
        {
            Title = "Sqlite Test Movie",
            TenantId = 1,
            RowVersion = new byte[8],
            ReleaseDate = DateTime.UtcNow,
            StudioId = studio.Id, // استفاده از ID استودیویی که در مرحله قبل ذخیره شد
            MovieDetail = new MovieDetail // اگر MovieDetail اجباری است
            {
                Language = "English",
                Country = "USA"
            }
        };

        // 3. ذخیره فیلم
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