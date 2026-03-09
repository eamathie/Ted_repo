namespace tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Models;

public interface IMovieRepository
{
    IQueryable<Movie> GetAll();
    Task<Movie?> GetMovie(string imdbId);
    Task Add(Movie movie);
    Task Delete(Movie movie);
    Task Update(Movie movie);
}