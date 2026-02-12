namespace tedMovieApp.Services.Interfaces;

public interface IMovieReviewService
{
    public Task<IEnumerable<Review>> GetAllMovieReviews();
    public Task<Review> GetMovieReview(int id);
    public Task<Review> CreateMovieReview(int movieId, string userId, string title, string reviewText, int stars);
    public Task<Review> DeleteMovieReview(int id, string userId, bool isAdmin);
    public Task<Review> UpdateMovieReview(int id, string userId, bool isAdmin, int movieId, string title, string reviewText, int stars);
}