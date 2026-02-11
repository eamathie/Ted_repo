namespace tedMovieApp.Services.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> SearchMovies(string title);
    Task<Movie> GetMovie(string id);
}