using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet(Name = "GetAllMovieReviews")]
    [Authorize]
    public async Task<ActionResult<Review>> GetAllMovieReviews()
    {
        var reviews = await _dbContext.Reviews.ToListAsync();
        return Ok(reviews);
    }

    [HttpPost(Name = "CreateMovieReview")]
    [Authorize]
    public async Task<ActionResult<Review>> CreateMovieReview(int movieId, string title, string reviewText, int stars)
    {
        var movie = await _dbContext.Movies.Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.MovieId == movieId);
        
        if (movie == null)
        {
            _logger.LogError($"Movie with id {movieId} not found");
            return NotFound($"Movie with ID {movieId} was not found.");
        }

        var review = new Review
        {
            Title = title,
            ReviewText = reviewText,
            Stars = stars,
            MovieId = movie.MovieId,
            Movie = movie
        };
        
        _dbContext.Reviews.Add(review); 
        await _dbContext.SaveChangesAsync(); 
        return Ok(review);
    }

    [HttpDelete(Name = "DeleteMovieReview")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Review>> DeleteMovieReview(int id)
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == id);
        if (review == null)
            return NotFound($"Review with id {id} does not exist");
        
        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();
        return Ok(review);
    }
}