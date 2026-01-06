using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MovieManagementSystem.Infrastructure.Data;
using MovieManagementSystem.Core.Entities;
using System.Net.Http.Json;
using Xunit;

namespace MovieManagementSystem.Tests;

public class MovieControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MovieControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // In a real scenario, you'd replace the real DB with InMemory here
            });
        });
    }

    [Fact]
    public async Task GetEagerSafe_ReturnsSuccessAndCorrectData()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/moviequery/eager-safe");

        // Assert
        response.EnsureSuccessStatusCode();
        var movies = await response.Content.ReadFromJsonAsync<List<dynamic>>();

        Assert.NotNull(movies);
        // Additional assertions based on your seed data in AppDbContext
    }
}