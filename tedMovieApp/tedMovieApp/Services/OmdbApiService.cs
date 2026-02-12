using System.Net;
using Microsoft.Extensions.Options;
using tedMovieApp.Services.Interfaces;
using tedMovieApp.Tools;

namespace tedMovieApp.Services;

public class OmdbApiService(ILogger<OmdbApiService> logger, IOptions<OmdbSettings> settings)
    : IOmdbApiService
{
    private readonly OmdbSettings _settings = settings.Value; 
    private readonly HttpClient _httpClient = new();

    public async Task<string> GetMoviesByQuery(string query)
    {
        var url = $"{_settings.BaseUrl}?apiKey={_settings.ApiKey}&s={query}"; 
        logger.LogInformation("Calling OMDB API: {url}", url); 
        
        return await _httpClient.GetStringAsync(url);
    }

    public async Task<string> GetMovieById(string id)
    {
        var url = $"{_settings.BaseUrl}?apiKey={_settings.ApiKey}&i={id}"; 
        logger.LogInformation("Calling OMDB API: {url}", url); 
        
        return await _httpClient.GetStringAsync(url);
    }
}