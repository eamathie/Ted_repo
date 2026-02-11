using System.Text.Json;
using tedMovieApp.Dtos;

namespace tedMovieApp.Services;

public class JsonProcessor : IJsonProcessor
{
    public Movie ProcessMovieResponse(string data)
    {
        /*
        JsonDocument json = JsonDocument.Parse(data);
        
        
        var root =  json.RootElement;
        //var first = root.GetProperty("Search")[0];
        //var search = json.RootElement.GetProperty("Search");
        
        var movie = new Movie
        {
            Title = root.GetProperty("Title").GetString(),
            Genre = root.GetProperty("Genre").GetString(),
            Length = root.GetProperty("Runtime").GetString(),
            Description = root.GetProperty("Plot").GetString(),
            ReleaseDate = DateOnly.Parse(root.GetProperty("Released").GetString())
        };
        
        return movie;
        */
        var json = JsonDocument.Parse(data); 
        var root = json.RootElement; 
        
        // OMDB returns { "Response": "False", "Error": "Movie not found!" }
        if (root.TryGetProperty("Response", out var responseProp) && responseProp.GetString() == "False")
            return null;
        
        return new Movie
        {
            ImdbId = root.GetProperty("imdbID").GetString(), 
            Title = root.GetProperty("Title").GetString(), 
            Genre = root.GetProperty("Genre").GetString(), 
            Length = root.GetProperty("Runtime").GetString(),
            Description = root.GetProperty("Plot").GetString(), 
            ReleaseDate = DateOnly.Parse(root.GetProperty("Released").GetString()), 
            PosterUrl = root.GetProperty("Poster").GetString()
        }; 
    }
    
    // Handles search results (Search: [ { Title, Year, imdbID } ])
    public IEnumerable<MovieDto> ProcessSearchResults(string data)
    {
        var json = JsonDocument.Parse(data); 
        var root = json.RootElement; 
        if (!root.TryGetProperty("Search", out var searchArray)) 
            return []; 
        
        var movies = new List<MovieDto>();
        foreach (var item in searchArray.EnumerateArray())
        {
            movies.Add(new MovieDto
            {
                ImdbId = item.GetProperty("imdbID").GetString(), 
                Title = item.GetProperty("Title").GetString(), 
                ReleaseYear = item.GetProperty("Year").GetString(), 
                PosterUrl = item.GetProperty("Poster").GetString()
            });
        } 
        
        return movies;
    }
}