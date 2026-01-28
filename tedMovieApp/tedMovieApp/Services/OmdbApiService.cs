using System.Net;

namespace tedMovieApp.Services;

public class OmdbApiService : IOmdbApiService
{
    private readonly ILogger<OmdbApiService> _logger;

    public OmdbApiService(ILogger<OmdbApiService> logger)
    {
        _logger = logger;
    }
    
    public string GetMovie(string movieTitle)
    {
        string key = "89a44e09";
        var url = $"http://www.omdbapi.com/?apiKey={key}&t={movieTitle}";//$"http://www.omdbapi.com/?apikey={key}&s={movieTitle}";
        //"http://www.omdbapi.com/?t=batman&plot=full"
            //"http://www.omdbapi.com/?apikey=[yourkey]&"
        
        using var client = new WebClient();
        _logger.LogInformation("Calling OMDB API with url: {url}", url);
        return client.DownloadString(url);
    }
}