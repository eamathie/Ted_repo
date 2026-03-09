using System.Text.Json.Serialization;

namespace tedMovieApp.Models.Dtos;

public class MovieDto
{
    [JsonPropertyName("imdbID")] public string ImdbId { get; set; }
    [JsonPropertyName("Title")] public string Title { get; set; }
    [JsonPropertyName("Year")] public string ReleaseYear { get; set; }
    [JsonPropertyName("Poster")] public string PosterUrl { get; set; }
}