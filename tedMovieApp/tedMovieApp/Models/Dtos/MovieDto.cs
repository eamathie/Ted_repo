using System.Text.Json.Serialization;

namespace tedMovieApp.Models.Dtos;

public class MovieDto
{
    [JsonPropertyName("imdbID")] public required string ImdbId { get; set; }
    [JsonPropertyName("Title")] public required string Title { get; set; }
    [JsonPropertyName("Year")] public required string ReleaseYear { get; set; }
    [JsonPropertyName("Poster")] public required string PosterUrl { get; set; }
}