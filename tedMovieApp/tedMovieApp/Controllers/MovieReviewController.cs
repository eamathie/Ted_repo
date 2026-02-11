using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tedMovieApp.Dtos; 
using tedMovieApp.Services.Interfaces;


namespace tedMovieApp.Controllers;

[ApiController]
[Route("api/reviews")]
public class MovieReviewController : ControllerBase
{
    private readonly IMovieReviewService _movieReviewService;
    
    public MovieReviewController(IMovieReviewService movieReviewService)
    {
        _movieReviewService = movieReviewService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Review>> GetAllMovieReviews()
    {
        var reviews = await _movieReviewService.GetAllMovieReviews();
        return Ok(reviews);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<Review>> GetMovieReview(int id)
    {
        var review = await _movieReviewService.GetMovieReview(id);
        return Ok(review);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Review>> CreateMovieReview(int movieId, ReviewDto dto)
    {
        var result = await _movieReviewService.CreateMovieReview(
            dto.MovieId,
            dto.Title,
            dto.ReviewText,
            dto.Stars
            ); 
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Review>> DeleteMovieReview(int id)
    {
        try
        {
            await _movieReviewService.DeleteMovieReview(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Review>> UpdateMovieReview(int id, ReviewDto dto)
    {
        try
        {
            var updatedReview = await _movieReviewService.UpdateMovieReview(
                id,
                dto.MovieId,
                dto.Title,
                dto.ReviewText,
                dto.Stars
            );
            
            return  Ok(updatedReview);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}