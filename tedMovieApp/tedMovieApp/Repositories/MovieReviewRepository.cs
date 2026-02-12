using Microsoft.EntityFrameworkCore;
using tedMovieApp.Repositories.Interfaces;

namespace tedMovieApp.Repositories;

public class MovieReviewRepository : IMovieReviewRepository
{
    private readonly MovieReviewApiContext _dbContext;

    public MovieReviewRepository(MovieReviewApiContext dbContext)
    {
        _dbContext =  dbContext;
    }
    
    public async Task<IEnumerable<Review>> GetAll()
    {
        return await _dbContext.Reviews
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<Review?> GetReview(int id)
    {
        return await _dbContext.Reviews.FirstOrDefaultAsync(review => review.ReviewId == id);
    }

    public async Task Add(Review review)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
    }

    public Task Delete(Review review)
    {
        _dbContext.Reviews.Remove(review);
        return _dbContext.SaveChangesAsync();
    }

    public Task Update(Review review)
    {
        _dbContext.Reviews.Update(review);
        return _dbContext.SaveChangesAsync();
    }
}