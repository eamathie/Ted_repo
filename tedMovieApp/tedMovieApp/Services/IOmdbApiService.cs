namespace tedMovieApp.Services;

public interface IOmdbApiService
{
    string GetMovie(string movieTitle);
}