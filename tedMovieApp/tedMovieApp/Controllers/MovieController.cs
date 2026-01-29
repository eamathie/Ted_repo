using Microsoft.AspNetCore.Authorization;
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
    private readonly MovieReviewApiContext _dbContext;

    public MovieController(
        ILogger<MovieController> logger, 
        IOmdbApiService omdbApiService,
        IJsonProcessor jsonProcessor,
        MovieReviewApiContext dbContext)
    {
        _logger = logger;
        _omdbApiService = omdbApiService;
        _jsonProcessor = jsonProcessor;
        _dbContext = dbContext;
    }

    [HttpGet("one", Name = "GetMovie")]
    public async Task<ActionResult<Movie>> GetMovie(string movieName)
    {
        var movie = await _dbContext.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Title == movieName);
        //var movie = dbContext.Movies.FirstOrDefault(m => m.Title == movieName);

        if (movie == null)
        {
            var data = _omdbApiService.GetMovie(movieName);
            movie = _jsonProcessor.ProcessMovieResponse(data);
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
        }
        
        return Ok(movie);
    }

    [HttpGet("all", Name = "GetAllMovies")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Movie>>> GetAllMovies()
    {
        //var movies = dbContext.Movies.ToList();
        var movies = await _dbContext.Movies.Include(m => m.Reviews) .ToListAsync();
        return Ok(movies);
    }
    
}