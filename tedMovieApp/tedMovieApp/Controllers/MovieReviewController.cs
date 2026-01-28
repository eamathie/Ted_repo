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
    
    public MovieReviewController(
        ILogger<MovieReviewController> logger, 
        IOmdbApiService omdbApiService,
        IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _omdbApiService = omdbApiService;
        _jsonProcessor = jsonProcessor;
    }
    
    [HttpGet(Name = "GetAllMovieReview")]
    public async Task<ActionResult<Review>> GetAllMovieReview()
    {
        await using var dbContext = new MovieReviewApiContext();
        var reviews = dbContext.Reviews;
        return Ok(reviews);
    }

    [HttpPost(Name = "CreateMovieReview")]
    public async Task<ActionResult<Review>> CreateMovieReview(string movieTitle, string title, string reviewText, int stars)
    {
        await using var dbContext = new MovieReviewApiContext();
        var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Title == movieTitle);
        if (movie == null)
        {
            var data = _omdbApiService.GetMovie(movieTitle);
            movie = _jsonProcessor.ProcessMovieResponse(data);
            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();
        }

        var review = new Review
        {
            Title = title,
            //Movie = movie,
            ReviewText = reviewText,
            Stars = stars
        };
        
        movie.Reviews.Add(review);
        dbContext.Reviews.Add(review); 
        await dbContext.SaveChangesAsync(); 
        return Ok(review);
    }
}