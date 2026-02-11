using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services.Interfaces;

namespace tedMovieApp.Services;

public class MovieReviewService : IMovieReviewService
{
    private readonly IMovieReviewRepository _movieReviewRepository;
    
    public MovieReviewService(IMovieReviewRepository movieReviewRepository)
    {
        _movieReviewRepository = movieReviewRepository;
    }

    public async Task<IEnumerable<Review>> GetAllMovieReviews()
    {
        var reviews = await _movieReviewRepository.GetAll();
        return reviews;
    }

    public async Task<Review> GetMovieReview(int id)
    {
        var review = await _movieReviewRepository.GetReview(id);
        return review ??  throw new InvalidOperationException($"Review with id {id} does not exist");
    }

    public async Task<Review> CreateMovieReview(int movieId, string userId, string title, string reviewText, int stars)
    {
        var review = new Review
        {
            Title = title,
            ReviewText = reviewText,
            Stars = stars,
            MovieId = movieId,
            UserId =  userId
        };

        await _movieReviewRepository.Add(review);
        return review;
    }
    public async Task<Review> DeleteMovieReview(int id)
    {
        var review = await _movieReviewRepository.GetReview(id);
        if (review == null)
            throw new InvalidOperationException($"Review with id {id} does not exist");
        
        await _movieReviewRepository.Delete(review);
        return review;
    }

    public async Task<Review> UpdateMovieReview(int id, int movieId, string title, string reviewText, int stars)
    {
        var review = await _movieReviewRepository.GetReview(id);
        if (review == null)
            throw new InvalidOperationException($"Review with id {id} does not exist");
        
        review.Title = title;
        review.ReviewText = reviewText;
        review.Stars = stars;
        review.MovieId = movieId;
        
        await _movieReviewRepository.Update(review);
        return review;
    }
}