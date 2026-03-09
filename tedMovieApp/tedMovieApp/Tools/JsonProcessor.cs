using System.Text.Json;
using tedMovieApp.Models.Dtos;
using tedMovieApp.Models;

namespace tedMovieApp.Tools;

public class JsonProcessor : IJsonProcessor
{
    private readonly JsonSerializerOptions options = new() {};
    public Movie? ProcessMovieResponse(string data)
    {
        options.Converters.Add(new OmdbDateOnlyConverter());
        
        var json = JsonDocument.Parse(data); 
        var root = json.RootElement; 
        
        // OMDB returns { "Response": "False", "Error": "Movie not found!" }
        if (root.TryGetProperty("Response", out var responseProp) && responseProp.GetString() == "False")
            return null;

        Movie? movie = JsonSerializer.Deserialize<Movie>(root, options);
        return movie;
    }
    
    // Handles search results (Search: [ { Title, Year, imdbID } ])
    public IEnumerable<MovieDto> ProcessSearchResults(string data)
    {
        var json = JsonDocument.Parse(data); 
        var root = json.RootElement; 
        if (!root.TryGetProperty("Search", out var searchArray) || searchArray.ValueKind != JsonValueKind.Array) 
            return []; 
        
        var movies = new List<MovieDto>();
        return searchArray
            .EnumerateArray()
            .Select(item => JsonSerializer.Deserialize<MovieDto>(item))
            .Where(m => m != null)!;
    }
}