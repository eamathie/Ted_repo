namespace tedMovieApp.Services.Interfaces;
using tedMovieApp.Models;

public interface IMovieService
{
    Task<IEnumerable<Movie>> SearchMovies(string title);
    Task<Movie> GetMovie(string id);
}