using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tedMovieApp.Services;


namespace tedMovieApp.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieReviewController : ControllerBase
{
    private readonly ILogger<MovieReviewController> _logger;
    private readonly IOmdbApiService _omdbApiService;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly MovieReviewApiContext _dbContext;
    
    public MovieReviewController(
        ILogger<MovieReviewController> logger, 
        IOmdbApiService omdbApiService,
        IJsonProcessor jsonProcessor,
        MovieReviewApiContext dbContext)
    {
        _logger = logger;
        _omdbApiService = omdbApiService;
        _jsonProcessor = jsonProcessor;
        _dbContext = dbContext;
    }
    
    [HttpGet(Name = "GetAllMovieReview")]
    public async Task<ActionResult<Review>> GetAllMovieReview()
    {
        var reviews = await _dbContext.Reviews.ToListAsync();
        return Ok(reviews);
    }

    [HttpPost(Name = "CreateMovieReview")]
    public async Task<ActionResult<Review>> CreateMovieReview(int movieId, string title, string reviewText, int stars)
    {
        var movie = await _dbContext.Movies .Include(m => m.Reviews) .FirstOrDefaultAsync(m => m.MovieId == movieId);
        //var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);
        if (movie == null)
        {
            _logger.LogError($"Movie with id {movieId} not found");
            return NotFound($"Movie with ID {movieId} was not found.");
            /*var data = _omdbApiService.GetMovie(movieTitle);
            movie = _jsonProcessor.ProcessMovieResponse(data);
            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();*/
        }

        var review = new Review
        {
            Title = title,
            //Movie = movie,
            ReviewText = reviewText,
            Stars = stars,
            MovieId = movie.MovieId,
            Movie = movie
        };
        
        //movie.Reviews.Add(review);
        _dbContext.Reviews.Add(review); 
        await _dbContext.SaveChangesAsync(); 
        return Ok(review);
    }
}