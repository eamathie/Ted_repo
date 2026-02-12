using System.Net;
using Microsoft.Extensions.Options;
using tedMovieApp.Tools;

namespace tedMovieApp.Services;

public class OmdbApiService : IOmdbApiService
{
    private readonly ILogger<OmdbApiService> _logger; 
    private readonly OmdbSettings _settings; 
    private readonly HttpClient _httpClient;

    public OmdbApiService(ILogger<OmdbApiService> logger, IOptions<OmdbSettings> settings)
    {
        _logger = logger; 
        _settings = settings.Value; 
        _httpClient = new HttpClient();
    }
    
    public async Task<string> GetMoviesByQuery(string query)
    {
        var url = $"{_settings.BaseUrl}?apiKey={_settings.ApiKey}&s={query}"; 
        _logger.LogInformation("Calling OMDB API: {url}", url); 
        
        return await _httpClient.GetStringAsync(url);
    }

    public async Task<string> GetMovieById(string id)
    {
        var url = $"{_settings.BaseUrl}?apiKey={_settings.ApiKey}&i={id}"; 
        _logger.LogInformation("Calling OMDB API: {url}", url); 
        
        return await _httpClient.GetStringAsync(url);
    }
}