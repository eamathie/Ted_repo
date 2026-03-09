using Microsoft.EntityFrameworkCore;
using tedMovieApp;
using tedMovieApp.Repositories;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services;
using tedMovieApp.Models;

namespace tedMovieTest
{
    public class MovieReviewServiceIntegrationTests
    {
        private MovieReviewApiContext _db;
        private IMovieReviewRepository _repo;
        private MovieReviewService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MovieReviewApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new MovieReviewApiContext(options);
            _repo = new MovieReviewRepository(_db); // real repository
            _service = new MovieReviewService(_repo);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        // helper to create reviews, otherwise code will be very verbose
        private Review CreateReview(
            int movieId = 1,
            string userId = "user1",
            string title = "Test Review",
            string text = "Great movie",
            int stars = 5)
        {
            return new Review
            {
                MovieId = movieId,
                UserId = userId,
                Title = title,
                ReviewText = text,
                Stars = stars
            };
        }
        
        [Test]
        public async Task CreateMovieReview_SavesToDatabase()
        {
            var result = await _service.CreateMovieReview(
                movieId: 10,
                userId: "user1",
                title: "Amazing",
                reviewText: "Loved it",
                stars: 5
            );

            var saved = await _db.Reviews.FirstOrDefaultAsync(r => r.ReviewId == result.ReviewId);

            Assert.That(saved, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(saved.Title, Is.EqualTo("Amazing"));
                Assert.That(saved.MovieId, Is.EqualTo(10));
            });
        }
        
        [Test]
        public async Task UpdateMovieReview_UpdatesDatabase()
        {
            var review = CreateReview();
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            await _service.UpdateMovieReview(
                id: review.ReviewId,
                userId: review.UserId,
                isAdmin: false,
                movieId: 99,
                title: "Updated Title",
                reviewText: "Updated Text",
                stars: 3
            );

            var saved = await _db.Reviews.FindAsync(review.ReviewId);

            Assert.That(saved, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(saved.Title, Is.EqualTo("Updated Title"));
                Assert.That(saved.ReviewText, Is.EqualTo("Updated Text"));
                Assert.That(saved.Stars, Is.EqualTo(3));
                Assert.That(saved.MovieId, Is.EqualTo(99));
            });
        }
        
        [Test]
        public async Task DeleteMovieReview_RemovesFromDatabase()
        {
            var review = CreateReview();
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            await _service.DeleteMovieReview(review.ReviewId, review.UserId, isAdmin: false);

            var exists = await _db.Reviews.AnyAsync(r => r.ReviewId == review.ReviewId);
            Assert.That(exists, Is.False);
        }
        
        [Test]
        public void DeleteMovieReview_Throws_WhenUnauthorized()
        {
            var review = CreateReview(userId: "owner");
            _db.Reviews.Add(review);
            _db.SaveChanges();

            Assert.That(
                async () => await _service.DeleteMovieReview(review.ReviewId, "otherUser", isAdmin: false),
                Throws.TypeOf<UnauthorizedAccessException>()
            );
        }
        
        [Test]
        public async Task GetMovieReview_ReturnsCorrectReview()
        {
            var review = CreateReview();
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            var result = await _service.GetMovieReview(review.ReviewId);

            Assert.Multiple(() =>
            {
                Assert.That(result.ReviewId, Is.EqualTo(review.ReviewId));
                Assert.That(result.Title, Is.EqualTo(review.Title));
            });
        }
        
        [Test]
        public void GetMovieReview_Throws_WhenNotFound()
        {
            Assert.That(
                async () => await _service.GetMovieReview(999),
                Throws.InvalidOperationException
            );
        }
    }
}
