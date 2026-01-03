using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Data;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Proxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLazyLoadingProxies());

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movie Management System API",
        Version = "v1",
        Description = "A professional movie management system built with .NET and EF Core",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com"
        }
    });

    // Optional: Include XML comments
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

/*
// For testing the Change Tracker only remove later
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
*/
/*
Console.WriteLine("=== Testing EF Core Change Tracker ===\n");
// 1. Added when a new entity is added
var newMovie = new Movie
{
    Title = "Test Movie - Change Tracker",
    Description = "This movie is created only for testing",
    ReleaseDate = DateTime.UtcNow,
    Rating = 7.5m,
    DurationMinutes = 120,
    StudioId = 1  // Assume a studio with Id = 1 exists
};

context.Movies.Add(newMovie);
Console.WriteLine($"1. After Add: State = {context.Entry(newMovie).State}");
// Output: Added

await context.SaveChangesAsync();  // Now OK because Main is async
Console.WriteLine($"   After SaveChanges: State = {context.Entry(newMovie).State}\n");
// Output: Unchanged

// 2. Modified when an existing entity is changed
var existingMovie = await context.Movies.FindAsync(1);  // Inception movie
if (existingMovie != null)
{
    existingMovie.Rating = 9.9m;  // Change value
    Console.WriteLine($"2. After changing Rating: State = {context.Entry(existingMovie).State}");
    // Output: Modified

    // SaveChanges has not been called yet the change is only tracked
}

// 3. Deleted when an entity is removed
if (existingMovie != null)
{
    context.Movies.Remove(existingMovie);
    Console.WriteLine($"3. After Remove: State = {context.Entry(existingMovie).State}\n");
    // Output: Deleted
}

// 4. Detached entity not tracked by the context
var detachedMovie = new Movie
{
    Id = 999,
    Title = "This movie is not tracked"
};

Console.WriteLine($"4. Entity not attached to context: State = {context.Entry(detachedMovie).State}");
// Output: Detached
*/
/*
var movies = await context.Movies
    .AsNoTracking()
    .Where(m => m.Rating > 8)
    .ToListAsync();

foreach (var m in movies)
{
    Console.WriteLine(context.Entry(m).State);  // Detached
}

var trackedMovies = await context.Movies
    .Where(m => m.Rating > 8)
    .ToListAsync();

foreach (var m in trackedMovies)
{
    Console.WriteLine(context.Entry(m).State);  // Unchanged
}
*/
/*
// Assume this JSON comes from the client
var updateDto = new { Id = 1, Title = "Updated Title", Rating = 9.9m };

// Bad approach dangerous over-posting
// var movie = updateDto.ToMovie();
// context.Update(movie);  // All fields are updated, even those not sent by the client

// Correct approach update only allowed fields
var existingMovie = await context.Movies.FindAsync(updateDto.Id);
if (existingMovie != null)
{
    existingMovie.Title = updateDto.Title;
    existingMovie.Rating = updateDto.Rating;
    await context.SaveChangesAsync();
}

// Advanced approach using Attach (for detached entities)
var detachedMovie = new Movie
{
    Id = updateDto.Id,
    Title = updateDto.Title,
    Rating = updateDto.Rating
};

context.Attach(detachedMovie);
context.Entry(detachedMovie).Property(m => m.Title).IsModified = true;
context.Entry(detachedMovie).Property(m => m.Rating).IsModified = true;

await context.SaveChangesAsync();


var movieToUpdate = new Movie { Id = 1, Title = "Fast Update", Rating = 10m };
context.Movies.Update(movieToUpdate);
context.Entry(movieToUpdate).State = EntityState.Modified;
*/
/*
// For big queries
var largeQuery = await context.Movies
    .AsNoTracking()
    .Include(m => m.Genres)
    .ToListAsync();

context.ChangeTracker.Clear();

context.ChangeTracker.DetectChanges();
Console.WriteLine($"Entities tracked: {context.ChangeTracker.Entries().Count()}");
Console.WriteLine("\n=== Test completed ===");
Console.WriteLine("To persist changes, call SaveChanges or restart the application.");
*/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Management System API V1");

        // Serve Swagger UI at root URL (/)
        c.RoutePrefix = string.Empty;

        // Set the HTML page title
        c.DocumentTitle = "Movie Management API";

        // Hide the models/schema section by default
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();  // Async Run
