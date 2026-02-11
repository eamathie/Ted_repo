using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tedMovieApp.Services;

namespace tedMovieApp.Controllers;

[ApiController]
[Route("api/movies")]
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

    //[Authorize]
    [HttpGet("search")] 
    public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies(string title) 
    { 
        // look in our own database first
        var localResults = await _dbContext.Movies
            .Where(m => m.Title.ToLower().Contains(title)) 
            .Include(m => m.Reviews) 
            .ToListAsync(); 
        
        // determine if local results are “good enough”
        if (localResults.Count != 0) 
        { 
            if (localResults.Count >= 5)
                return Ok(localResults);
        } 
        
        // fetch from OMDB if needed
        var omdbData = await _omdbApiService.GetMoviesByQuery(title); 
        var parsedMoviesSimple = _jsonProcessor.ProcessSearchResults(omdbData); // these do not contain all information
        var parsedMoviesFull = new List<Movie>();
        
        // store new movies in DB (avoid duplicates)
        foreach (var movie in parsedMoviesSimple)
        {
            if (!await _dbContext.Movies.AnyAsync(m => m.ImdbId == movie.ImdbId))
            {
                var omdbDataFullDetails = await _omdbApiService.GetMovieById(movie.ImdbId);
                var movieWithFullDetails = _jsonProcessor.ProcessMovieResponse(omdbDataFullDetails);
                parsedMoviesFull.Add(movieWithFullDetails);
                _dbContext.Movies.Add(movieWithFullDetails);
            }
        } 
        await _dbContext.SaveChangesAsync(); 
        
        // return combined results (local + OMDB)
        var combined = localResults
            .Concat(parsedMoviesFull)
            .DistinctBy(m => m.ImdbId); 
        
        return Ok(combined); 
    }
    
    /*
    [HttpGet($"{{id}}")]
    public async Task<ActionResult<Movie>> GetMovie(string id)
    {
        var movie = await _dbContext.Movies.Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Title == movieName);

        if (movie == null)
        {
            var data = _omdbApiService.GetMovie(movieName);
            movie = _jsonProcessor.ProcessMovieResponse(data);
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
        }

        return Ok(movie);
        
    }
    
    */

    //[Authorize]
    [HttpGet("{query}")]
    public async Task<ActionResult<List<Movie>>> GetMovies(string query)
    {
        //throw  new NotImplementedException();
        var movies = await _dbContext.Movies.Include(m => m.Reviews) .ToListAsync();
        return Ok(movies);
    }
    
}