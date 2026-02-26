using tedMovieApp.Services;

namespace tedMovieTest
{
    public class JsonProcessorTests
    {
        private JsonProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new JsonProcessor();
        }
        
        [Test]
        public void ProcessMovieResponse_ReturnsNull_WhenResponseIsFalse()
        {
            const string json = """
                                {
                                    "Response": "False",
                                    "Error": "Movie not found!"
                                }
                                """;

            var result = _processor.ProcessMovieResponse(json);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ProcessMovieResponse_ParsesMovieCorrectly()
        {
            const string json = """
                                {
                                    "imdbID": "tt1234567",
                                    "Title": "Test Movie",
                                    "Genre": "Action",
                                    "Runtime": "120 min",
                                    "Plot": "A test plot.",
                                    "Released": "01 Jan 2020",
                                    "Poster": "http://example.com/poster.jpg"
                                }
                                """;

            var movie = _processor.ProcessMovieResponse(json);

            Assert.Multiple(() =>
            {
                Assert.That(movie.ImdbId, Is.EqualTo("tt1234567"));
                Assert.That(movie.Title, Is.EqualTo("Test Movie"));
                Assert.That(movie.Genre, Is.EqualTo("Action"));
                Assert.That(movie.Length, Is.EqualTo("120 min"));
            });
            Assert.Multiple(() =>
            {
                Assert.That(movie.Description, Is.EqualTo("A test plot."));
                Assert.That(movie.ReleaseDate, Is.EqualTo(new DateOnly(2020, 1, 1)));
                Assert.That(movie.PosterUrl, Is.EqualTo("http://example.com/poster.jpg"));
            });

        }

        [Test]
        public void ProcessSearchResults_ReturnsEmpty_WhenSearchMissing()
        {
            const string json = """
                                {
                                    "Response": "True"
                                }
                                """;

            var results = _processor.ProcessSearchResults(json);
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void ProcessSearchResults_ParsesSearchArrayCorrectly()
        {
            const string json = """
                                {
                                    "Search": [
                                        {
                                            "Title": "Movie A",
                                            "Year": "2020",
                                            "imdbID": "tt001",
                                            "Poster": "posterA.jpg"
                                        },
                                        {
                                            "Title": "Movie B",
                                            "Year": "2021",
                                            "imdbID": "tt002",
                                            "Poster": "posterB.jpg"
                                        }
                                    ]
                                }
                                """;

            var results = _processor.ProcessSearchResults(json).ToList();

            Assert.That(results, Has.Count.EqualTo(2));

            Assert.Multiple(() =>
            {
                Assert.That(results[0].ImdbId, Is.EqualTo("tt001"));
                Assert.That(results[0].Title, Is.EqualTo("Movie A"));
                Assert.That(results[0].ReleaseYear, Is.EqualTo("2020"));
                Assert.That(results[0].PosterUrl, Is.EqualTo("posterA.jpg"));

                Assert.That(results[1].ImdbId, Is.EqualTo("tt002"));
                Assert.That(results[1].Title, Is.EqualTo("Movie B"));
            });

        }
    }
}
