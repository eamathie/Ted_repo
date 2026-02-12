using Microsoft.EntityFrameworkCore;
using tedMovieApp.Repositories.Interfaces;

namespace tedMovieApp.Repositories;

public class MovieReviewRepository(MovieReviewApiContext dbContext) : IMovieReviewRepository
{
    public async Task<IEnumerable<Review>> GetAll()
    {
        return await dbContext.Reviews
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<Review?> GetReview(int id)
    {
        return await dbContext.Reviews.FirstOrDefaultAsync(review => review.ReviewId == id);
    }

    public async Task Add(Review review)
    {
        dbContext.Reviews.Add(review);
        await dbContext.SaveChangesAsync();
    }

    public Task Delete(Review review)
    {
        dbContext.Reviews.Remove(review);
        return dbContext.SaveChangesAsync();
    }

    public Task Update(Review review)
    {
        dbContext.Reviews.Update(review);
        return dbContext.SaveChangesAsync();
    }
}