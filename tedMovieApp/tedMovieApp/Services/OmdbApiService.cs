using System.Net;

namespace tedMovieApp.Services;

public class OmdbApiService : IOmdbApiService
{
    private readonly ILogger<OmdbApiService> _logger;
    private const string _key = "89a44e09";

    public OmdbApiService(ILogger<OmdbApiService> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetMoviesByQuery(string query)
    {
        var url = $"http://www.omdbapi.com/?apiKey={_key}&s={query}"; 
        using var client = new HttpClient(); 
        _logger.LogInformation("Calling OMDB API with url: {url}", url); 
        
        return await client.GetStringAsync(url);
    }

    public async Task<string> GetMovieById(string id)
    {
        var url = $"http://www.omdbapi.com/?apiKey={_key}&i={id}";
        using var client = new HttpClient();
        _logger.LogInformation("Calling OMDB API with url: {url}", url);
        
        return  await client.GetStringAsync(url);
    }
}