using Microsoft.EntityFrameworkCore;
using tedMovieApp.Repositories.Interfaces;

namespace tedMovieApp.Repositories;

public class MovieRepository(MovieReviewApiContext dbContext) : IMovieRepository
{
    public IQueryable<Movie> GetAll()
    {
        return dbContext.Movies.AsQueryable();
    }

    public async Task<Movie?> GetMovie(string imdbId)
    {
        return await dbContext.Movies.FirstOrDefaultAsync(movie => movie.ImdbId == imdbId);
    }

    public async Task Add(Movie movie)
    {
        dbContext.Movies.Add(movie);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Movie movie)
    {
        dbContext.Movies.Remove(movie);
        await dbContext.SaveChangesAsync();
    }

    public Task Update(Movie movie)
    {
        dbContext.Movies.Update(movie);
        return dbContext.SaveChangesAsync();
    }
}