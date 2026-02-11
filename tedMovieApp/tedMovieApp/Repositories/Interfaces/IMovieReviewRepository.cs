namespace tedMovieApp.Repositories.Interfaces;

public interface IMovieReviewRepository
{
    Task<IEnumerable<Review>> GetAll();
    Task<Review?> GetReview(int id);
    Task Add(Review review);
    Task Delete(Review review);
    Task Update(Review review);

}