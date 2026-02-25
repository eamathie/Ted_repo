using NSubstitute;
using tedMovieApp;
using tedMovieApp.Services;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services.Interfaces;

namespace tedMovieTest
{
    public class MovieServiceTests
    {
        private IMovieRepository _repo;
        private IOmdbApiService _omdb;
        private IJsonProcessor _json;
        private MovieService _service;

        [SetUp]
        public void Setup()
        {
            _repo = Substitute.For<IMovieRepository>();
            _omdb = Substitute.For<IOmdbApiService>();
            _json = Substitute.For<IJsonProcessor>();

            _service = new MovieService(_repo, _omdb, _json);
        }
        
        [Test]
        public async Task GetMovie_ReturnsMovie_WhenFound()
        {
            var movie = new Movie { ImdbId = "tt123" };
            _repo.GetMovie("tt123").Returns(movie);

            var result = await _service.GetMovie("tt123");
            Assert.That(result, Is.EqualTo(movie));
        }
        
        [Test]
        public void GetMovie_Throws_WhenNotFound()
        {
            _repo.GetMovie("missing").Returns((Movie)null);

            Assert.That(
                async () => await _service.GetMovie("missing"),
                Throws.InvalidOperationException
            );
        }
    }
}
