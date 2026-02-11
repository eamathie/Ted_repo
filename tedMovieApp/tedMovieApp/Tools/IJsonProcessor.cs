using tedMovieApp.Dtos;

namespace tedMovieApp.Services;

public interface IJsonProcessor
{
    Movie ProcessMovieResponse(string data);
    IEnumerable<MovieDto> ProcessSearchResults(string data);
}