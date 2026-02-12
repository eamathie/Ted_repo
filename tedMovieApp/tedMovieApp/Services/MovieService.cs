using Microsoft.EntityFrameworkCore;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services.Interfaces;

namespace tedMovieApp.Services;

public class MovieService(
    IMovieRepository movieRepository,
    IOmdbApiService omdbApiService,
    IJsonProcessor jsonProcessor)
    : IMovieService
{
    public async Task<IEnumerable<Movie>> SearchMovies(string title) 
    { 
        // look in our own database first
        var localResults = await movieRepository
            .GetAll()
            .Where(m => m.Title.ToLower().Contains(title.ToLower()))
            .Include(m => m.Reviews)
            .ToListAsync();
        
        // determine if local results are “good enough”
        if (localResults.Count >= 5) 
            return localResults;
        
        // fetch from OMDB if needed
        var omdbData = await omdbApiService.GetMoviesByQuery(title); 
        var parsedMoviesSimple = jsonProcessor.ProcessSearchResults(omdbData); // these do not contain all information
        var parsedMoviesFull = new List<Movie>();
        
        // store new movies in DB (avoid duplicates)
        foreach (var movie in parsedMoviesSimple)
        {
            if (!await movieRepository.GetAll().AnyAsync(m => m.ImdbId == movie.ImdbId))
            {
                var omdbDataFullDetails = await omdbApiService.GetMovieById(movie.ImdbId);
                var movieWithFullDetails = jsonProcessor.ProcessMovieResponse(omdbDataFullDetails);
                parsedMoviesFull.Add(movieWithFullDetails);
                await movieRepository.Add(movieWithFullDetails);
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
        var movie = await movieRepository.GetMovie(id);
        return movie ?? throw new InvalidOperationException($"Movie with id {id} not found");
    }
}