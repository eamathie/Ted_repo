using NSubstitute;
using tedMovieApp.Models;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services;

namespace tedMovieTest
{
    public class MovieReviewServiceTests
    {
        private IMovieReviewRepository _repo;
        private MovieReviewService _service;

        [SetUp]
        public void Setup()
        {
            _repo = Substitute.For<IMovieReviewRepository>();
            _service = new MovieReviewService(_repo);
        }
        
        [Test]
        public async Task GetAllMovieReviews_ReturnsRepositoryResults()
        {
            var reviews = new List<Review> { new(), new() };
            _repo.GetAll().Returns(reviews);

            var result = await _service.GetAllMovieReviews();

            Assert.That(result, Is.EqualTo(reviews));
        }
        
        [Test]
        public void GetMovieReview_Throws_WhenNotFound()
        {
            _repo.GetReview(1).Returns((Review)null);

            Assert.That(
                async () => await _service.GetMovieReview(1),
                Throws.InvalidOperationException
            );
        }
        
        [Test]
        public async Task GetMovieReview_ReturnsReview_WhenFound()
        {
            var review = new Review { ReviewId = 1 };
            _repo.GetReview(1).Returns(review);

            var result = await _service.GetMovieReview(1);

            Assert.That(result, Is.EqualTo(review));
        }

        [Test]
        public async Task GetMovieReviewsUser_ReturnsUserReviews()
        {
            var reviews = new List<Review> { new() };
            _repo.GetAllUser("abc").Returns(reviews);

            var result = await _service.GetMovieReviewsUser("abc");

            Assert.That(result, Is.EqualTo(reviews));
        }
        
        [Test]
        public async Task CreateMovieReview_CreatesAndReturnsReview()
        {
            Review? captured = null;

            _repo.Add(Arg.Do<Review>(r => captured = r))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateMovieReview(
                movieId: 10,
                userId: "u1",
                title: "Great!",
                reviewText: "Loved it",
                stars: 5
            );

            Assert.Multiple(() =>
            {
                Assert.That(captured.Title, Is.EqualTo("Great!"));
                Assert.That(captured.MovieId, Is.EqualTo(10));
                Assert.That(captured.UserId, Is.EqualTo("u1"));
                Assert.That(captured.Stars, Is.EqualTo(5));

                Assert.That(result, Is.EqualTo(captured));
            });
        }
        
        [Test]
        public void DeleteMovieReview_Throws_WhenNotFound()
        {
            _repo.GetReview(1).Returns((Review)null);

            Assert.That(
                async () => await _service.DeleteMovieReview(1, "u1", false),
                Throws.InvalidOperationException
            );
        }
        
        [Test]
        public void DeleteMovieReview_Throws_WhenUnauthorized()
        {
            var review = new Review { ReviewId = 1, UserId = "owner" };
            _repo.GetReview(1).Returns(review);

            Assert.That(
                async () => await _service.DeleteMovieReview(1, "otherUser", false),
                Throws.TypeOf<UnauthorizedAccessException>()
            );
        }
        
        [Test]
        public async Task DeleteMovieReview_Deletes_WhenAuthorized()
        {
            var review = new Review { ReviewId = 1, UserId = "u1" };
            _repo.GetReview(1).Returns(review);

            var result = await _service.DeleteMovieReview(1, "u1", false);

            await _repo.Received(1).Delete(review);
            Assert.That(result, Is.EqualTo(review));
        }
        
        [Test]
        public void UpdateMovieReview_Throws_WhenNotFound()
        {
            _repo.GetReview(1).Returns((Review)null);

            Assert.That(
                async () => await _service.UpdateMovieReview(1, "u1", false, 10, "t", "txt", 4),
                Throws.InvalidOperationException
            );
        }
        
        [Test]
        public void UpdateMovieReview_Throws_WhenUnauthorized()
        {
            var review = new Review { ReviewId = 1, UserId = "owner" };
            _repo.GetReview(1).Returns(review);

            Assert.That(
                async () => await _service.UpdateMovieReview(1, "other", false, 10, "t", "txt", 4),
                Throws.TypeOf<UnauthorizedAccessException>()
            );
        }
        
        [Test]
        public async Task UpdateMovieReview_Updates_WhenAuthorized()
        {
            var review = new Review { ReviewId = 1, UserId = "u1" };
            _repo.GetReview(1).Returns(review);

            var result = await _service.UpdateMovieReview(
                id: 1,
                userId: "u1",
                isAdmin: false,
                movieId: 99,
                title: "Updated",
                reviewText: "New text",
                stars: 3
            );

            Assert.Multiple(() =>
            {
                Assert.That(review.Title, Is.EqualTo("Updated"));
                Assert.That(review.ReviewText, Is.EqualTo("New text"));
                Assert.That(review.Stars, Is.EqualTo(3));
                Assert.That(review.MovieId, Is.EqualTo(99));
            });

            await _repo.Received(1).Update(review);
            Assert.That(result, Is.EqualTo(review));
        }
    }
}