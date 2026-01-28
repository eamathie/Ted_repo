using Microsoft.AspNetCore.Mvc;


namespace tedMovieApp.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieReviewController : ControllerBase
{
    [HttpGet(Name = "GetAllMovieReview")]
    public async Task<ActionResult<Review>> GetAllMovieReview()
    {
        await using var dbContext = new MovieReviewApiContext();
        var reviews = dbContext.Reviews;
        return Ok(reviews);
    }
}