using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tedMovieApp.Services;
using tedMovieApp.Services.Interfaces;

namespace tedMovieApp.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController(IMovieService movieService) : ControllerBase
{
    [Authorize]
    [HttpGet("search")] 
    public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies(string title) 
    { 
        var movies = await movieService.SearchMovies(title);
        return Ok(movies);
    }
    
    [Authorize]
    [HttpGet($"{{id}}")]
    public async Task<ActionResult<Movie>> GetMovie(string id)
    {
        var movie = await movieService.GetMovie(id);
        return Ok(movie);
    }
}