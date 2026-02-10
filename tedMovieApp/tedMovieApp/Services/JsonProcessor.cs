using System.Text.Json;

namespace tedMovieApp.Services;

public class JsonProcessor : IJsonProcessor
{
    public Movie ProcessMovieResponse(string data)
    {
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
    }
}