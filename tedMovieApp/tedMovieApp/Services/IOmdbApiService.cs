namespace tedMovieApp.Services;

public interface IOmdbApiService
{
    Task<string> GetMoviesByQuery(string query);
    Task<string> GetMovieById(string id);



}