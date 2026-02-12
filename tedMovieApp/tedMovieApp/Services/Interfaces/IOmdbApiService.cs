namespace tedMovieApp.Services.Interfaces;

public interface IOmdbApiService
{
    Task<string> GetMoviesByQuery(string query);
    Task<string> GetMovieById(string id);



}