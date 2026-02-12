using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services.Interfaces;

namespace tedMovieApp.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IOmdbApiService _omdbApiService;
    private readonly IJsonProcessor _jsonProcessor;
    
    public MovieService(IMovieRepository movieRepository, IOmdbApiService omdbApiService, IJsonProcessor jsonProcessor)
    {
        _movieRepository = movieRepository;
        _omdbApiService = omdbApiService;
        _jsonProcessor = jsonProcessor;
    }
    
    
    public async Task<IEnumerable<Movie>> SearchMovies(string title) 
    { 
        // look in our own database first
        var localResults = await _movieRepository
            .GetAll()
            .Where(m => m.Title.ToLower().Contains(title.ToLower()))
            .Include(m => m.Reviews)
            .ToListAsync();
        
        // determine if local results are “good enough”
        if (localResults.Count >= 5) 
            return localResults;
        
        // fetch from OMDB if needed
        var omdbData = await _omdbApiService.GetMoviesByQuery(title); 
        var parsedMoviesSimple = _jsonProcessor.ProcessSearchResults(omdbData); // these do not contain all information
        var parsedMoviesFull = new List<Movie>();
        
        // store new movies in DB (avoid duplicates)
        foreach (var movie in parsedMoviesSimple)
        {
            if (!await _movieRepository.GetAll().AnyAsync(m => m.ImdbId == movie.ImdbId))
            {
                var omdbDataFullDetails = await _omdbApiService.GetMovieById(movie.ImdbId);
                var movieWithFullDetails = _jsonProcessor.ProcessMovieResponse(omdbDataFullDetails);
                parsedMoviesFull.Add(movieWithFullDetails);
                await _movieRepository.Add(movieWithFullDetails);
            }
        } 
        
        // return combined results (local + OMDB)
        var combined = localResults
            .Concat(parsedMoviesFull)
            .DistinctBy(m => m.ImdbId); 
        
        return combined; 
    }
    
    public async Task<Movie> GetMovie(string id)
    {
        var movie = await _movieRepository.GetMovie(id);
        return movie ?? throw new InvalidOperationException($"Movie with id {id} not found");
    }
}