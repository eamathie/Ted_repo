namespace tedMovieApp.Repositories.Interfaces;

public interface IMovieReviewRepository
{
    Task<IEnumerable<Review>> GetAll();
    Task<Review?> GetReview(int id);

    Task<IEnumerable<Review>> GetAllUser(string id);
    Task Add(Review review);
    Task Delete(Review review);
    Task Update(Review review);

}