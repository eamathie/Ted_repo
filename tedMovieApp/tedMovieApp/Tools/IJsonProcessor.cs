using tedMovieApp.Dtos;

namespace tedMovieApp.Tools;

public interface IJsonProcessor
{
    Movie ProcessMovieResponse(string data);
    IEnumerable<MovieDto> ProcessSearchResults(string data);
}