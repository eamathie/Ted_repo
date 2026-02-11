using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tedMovieApp.Services;
using tedMovieApp.Services.Interfaces;

namespace tedMovieApp.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    //[Authorize]
    [HttpGet("search")] 
    public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies(string title) 
    { 
        var movies = await _movieService.SearchMovies(title);
        return Ok(movies);
    }
    
    [Authorize]
    [HttpGet($"{{id}}")]
    public async Task<ActionResult<Movie>> GetMovie(string id)
    {
        var movie = await _movieService.GetMovie(id);
        return Ok(movie);
    }
}