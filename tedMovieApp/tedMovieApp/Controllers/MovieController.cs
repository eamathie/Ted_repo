using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tedMovieApp.Services;

namespace tedMovieApp.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly IOmdbApiService _omdbApiService;
    private readonly IJsonProcessor _jsonProcessor;

    public MovieController(
        ILogger<MovieController> logger, 
        IOmdbApiService omdbApiService,
        IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _omdbApiService = omdbApiService;
        _jsonProcessor = jsonProcessor;
    }

    [HttpGet("one", Name = "GetMovie")]
    public async Task<ActionResult<Movie>> GetMovie(string movieName)
    {
        await using var dbContext = new MovieReviewApiContext();
        var movie = await dbContext.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Title == movieName);
        //var movie = dbContext.Movies.FirstOrDefault(m => m.Title == movieName);

        if (movie == null)
        {
            var data = _omdbApiService.GetMovie(movieName);
            movie = _jsonProcessor.ProcessMovieResponse(data);
            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();
        }
        
        return Ok(movie);
    }

    [HttpGet("all", Name = "GetAllMovies")]
    public async Task<ActionResult<List<Movie>>> GetAllMovies()
    {
        await using var dbContext = new MovieReviewApiContext();
        //var movies = dbContext.Movies.ToList();
        var movies = await dbContext.Movies.Include(m => m.Reviews) .ToListAsync();
        return Ok(movies);
    }
    
}