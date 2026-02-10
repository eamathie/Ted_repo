namespace tedMovieApp.Services;

public interface IJsonProcessor
{
    Movie ProcessMovieResponse(string data);
}