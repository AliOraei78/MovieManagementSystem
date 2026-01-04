using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Core.Entities;
using MovieManagementSystem.Infrastructure.Data;
using MovieManagementSystem.Infrastructure.Services;

namespace MovieManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieQueryController : ControllerBase
{
    private readonly AppDbContext _context;

    public MovieQueryController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Full Eager Loading test – all related entities are loaded in a single query
    /// </summary>
    [HttpGet("eager-safe")]
    public async Task<IActionResult> GetWithSafeProjection()
    {
        var movies = await _context.Movies
            .Include(m => m.Studio)
            .Include(m => m.Genres)
            .Include(m => m.Actors)
            .Include(m => m.MovieDetail)
            .Select(m => new
            {
                m.Id,
                m.Title,
                m.ReleaseDate,
                m.Rating,
                m.DurationMinutes,
                Studio = new
                {
                    m.Studio.Id,
                    m.Studio.Name,
                    m.Studio.Country
                },
                Genres = m.Genres.Select(g => new { g.Id, g.Name }).ToList(),
                Actors = m.Actors.Select(a => new { a.Id, a.Name }).ToList(),
                MovieDetail = m.MovieDetail != null ? new
                {
                    m.MovieDetail.Language,
                    m.MovieDetail.Country,
                    m.MovieDetail.Budget,
                    m.MovieDetail.Revenue
                } : null
            })
            .ToListAsync();

        return Ok(movies);
    }

    /// <summary>
    /// Eager Loading test using ThenInclude (for deeper relationships)
    /// Hypothetical example: if Genre had SubGenres
    /// </summary>
    [HttpGet("theninclude")]
    public async Task<IActionResult> GetWithThenInclude()
    {
        var movies = await _context.Movies
            .Include(m => m.Studio)
            .Include(m => m.Genres)
            // .ThenInclude(g => g.SubGenres)  // If sub-genres existed
            .Include(m => m.Actors)
            .Include(m => m.MovieDetail)
            .AsSplitQuery()
            .ToListAsync();

        return Ok(movies);
    }

    /// <summary>
    /// Test without Include – movies only (used to demonstrate N+1 problem)
    /// </summary>
    [HttpGet("no-include")]
    public async Task<IActionResult> GetWithoutInclude()
    {
        var movies = await _context.Movies
                    .Include(m => m.Genres)
                    .Include(m => m.Actors)
                    .AsSplitQuery()
                    .ToListAsync();

        // If Genres or other navigation properties are accessed
        // in the view or serializer → N+1 problem occurs
        return Ok(movies);
    }

    [HttpGet("projection")]
    public async Task<IActionResult> GetWithProjection()
    {
        var movieDtos = await _context.Movies
            .Select(m => new
            {
                m.Title,
                m.ReleaseDate,
                m.Rating,
                Studio = m.Studio.Name,
                Genres = m.Genres.Select(g => g.Name).ToList(),
                Actors = m.Actors.Select(a => a.Name).ToList()
            })
            .ToListAsync();

        return Ok(movieDtos);
    }

    [HttpGet("explicit/{id}")]
    public async Task<IActionResult> GetWithExplicitLoading(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return NotFound();

        // Explicitly load Genres
        await _context.Entry(movie)
                      .Collection(m => m.Genres)
                      .LoadAsync();

        // Explicitly load Actors
        await _context.Entry(movie)
                      .Collection(m => m.Actors)
                      .LoadAsync();

        // Explicitly load Studio
        await _context.Entry(movie)
                      .Reference(m => m.Studio)
                      .LoadAsync();

        return Ok(movie);
    }

    [HttpGet("asnotracking")]
    public async Task<IActionResult> GetWithAsNoTracking()
    {
        var movies = await _context.Movies
            .AsNoTracking()  // ← مهم: tracking خاموش می‌شود
            .Include(m => m.Studio)
            .Include(m => m.Genres)
            .Include(m => m.Actors)
            .ToListAsync();

        return Ok(movies);
    }

    [HttpGet("compiled/{id}")]
    public async Task<IActionResult> GetMovieCompiled(int id)
    {
        var movie = await MovieQueries.GetMovieByIdAsync(_context, id);

        if (movie == null) return NotFound();

        return Ok(movie);
    }

}
