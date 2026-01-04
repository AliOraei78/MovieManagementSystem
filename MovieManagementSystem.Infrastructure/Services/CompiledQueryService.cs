using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Data;

namespace MovieManagementSystem.Infrastructure.Services;

public static class MovieQueries
{
    // Compiled Query - compiled once and executed many times for better performance
    private static readonly Func<AppDbContext, int, Task<Movie?>> GetMovieByIdCompiled =
        EF.CompileAsyncQuery((AppDbContext context, int id) =>
            context.Movies
                .Include(m => m.Studio)
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .FirstOrDefault(m => m.Id == id));

    public static Task<Movie?> GetMovieByIdAsync(AppDbContext context, int id)
    {
        return GetMovieByIdCompiled(context, id);
    }
}
