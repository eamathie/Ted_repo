using Microsoft.AspNetCore.Mvc;
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

    [HttpGet(Name = "GetMovie")]
    public async Task<ActionResult<Movie>> GetMovie(string movieName)
    {
        await using var dbContext = new MovieReviewApiContext();
        var movie = dbContext.Movies.FirstOrDefault(m => m.Title == movieName);

        if (movie == null)
        {
            var data = _omdbApiService.GetMovie(movieName);
            movie = _jsonProcessor.ProcessMovieResponse(data);
            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();
        }
        
        return Ok(movie);
    }
    
}