using Microsoft.EntityFrameworkCore;
using NSubstitute;
using tedMovieApp;
using tedMovieApp.Dtos;
using tedMovieApp.Repositories;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services;
using tedMovieApp.Services.Interfaces;

namespace tedMovieTest
{
    public class MovieServiceIntegrationTests
    {
        private MovieReviewApiContext _db;
        private IMovieRepository _repo;
        private IOmdbApiService _omdb;
        private IJsonProcessor _json;
        private MovieService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MovieReviewApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB per test
                .Options;

            _db = new MovieReviewApiContext(options);
            _repo = new MovieRepository(_db);
            _omdb = Substitute.For<IOmdbApiService>();
            _json = Substitute.For<IJsonProcessor>();

            _service = new MovieService(_repo, _omdb, _json);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        // helper method, because otherwise this becomes very repetitive and verbose
        private Movie CreateMovie(string imdbId, string title)
        {
            return new Movie
            {
                ImdbId = imdbId,
                Title = title,
                Description = "Test description",
                Genre = "Action",
                Length = "120 min",
                PosterUrl = "http://example.com/poster.jpg",
                ReleaseDate = new DateOnly(2020, 1, 1)
            };
        }
        
        [Test]
        public async Task SearchMovies_ReturnsLocalResults_WhenEnoughLocal()
        {
            for (var i = 1; i <= 5; i++)
                _db.Movies.Add(CreateMovie($"id{i}", $"Movie {i}"));

            await _db.SaveChangesAsync();

            var result = await _service.SearchMovies("movie");

            Assert.That(result.Count(), Is.EqualTo(5));
            await _omdb.DidNotReceive().GetMoviesByQuery(Arg.Any<string>());
        }
        
        [Test]
        public async Task SearchMovies_FetchesFromOmdb_AndStoresNewMovies()
        {
            _db.Movies.Add(CreateMovie("local1", "Local Movie"));
            await _db.SaveChangesAsync();

            _omdb.GetMoviesByQuery("batman").Returns("search-json");

            _json.ProcessSearchResults("search-json").Returns([
                new MovieDto { ImdbId = "tt001", Title = "Batman Begins" }
            ]);

            _omdb.GetMovieById("tt001").Returns("full-json");

            _json.ProcessMovieResponse("full-json").Returns(
                CreateMovie("tt001", "Batman Begins")
            );

            await _service.SearchMovies("batman");

            Assert.Multiple(() =>
            {
                Assert.That(_db.Movies.Count(), Is.EqualTo(2));
                Assert.That(_db.Movies.Any(m => m.ImdbId == "tt001"), Is.True);
            });
        }
        
        [Test]
        public async Task SearchMovies_DoesNotAddDuplicates()
        {
            _db.Movies.Add(CreateMovie("tt001", "Local Batman"));
            await _db.SaveChangesAsync();

            _omdb.GetMoviesByQuery("batman").Returns("search-json");

            _json.ProcessSearchResults("search-json").Returns([
                new MovieDto { ImdbId = "tt001", Title = "Batman Begins" }
            ]);

            await _service.SearchMovies("batman");

            Assert.That(_db.Movies.Count(), Is.EqualTo(1));
            await _omdb.DidNotReceive().GetMovieById("tt001");
        }
    }
}
