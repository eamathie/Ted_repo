using Microsoft.EntityFrameworkCore;
using tedMovieApp.Repositories.Interfaces;

namespace tedMovieApp.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MovieReviewApiContext _dbContext;

    public MovieRepository(MovieReviewApiContext dbContext)
    {
        _dbContext =  dbContext;
    }
    
    public IQueryable<Movie> GetAll()
    {
        return _dbContext.Movies.AsQueryable();
    }

    public async Task<Movie?> GetMovie(string imdbId)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.ImdbId == imdbId);
    }

    public async Task Add(Movie movie)
    {
        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Movie movie)
    {
        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(Movie movie)
    {
        _dbContext.Movies.Update(movie);
        return _dbContext.SaveChangesAsync();
    }
}