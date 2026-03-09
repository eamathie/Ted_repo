using System.Text.Json.Serialization;

namespace tedMovieApp.Models;

public class Movie
{
    public int MovieId { get; set; }
    [JsonPropertyName("imdbID")] public required string ImdbId  { get; set; }
    [JsonPropertyName("Title")] public required string Title { get; set; }
    [JsonPropertyName("Genre")] public required string Genre { get; set; }
    [JsonPropertyName("Runtime")] public required string Length { get; set; }
    [JsonPropertyName("Plot")] public required string Description { get; set; }
    [JsonPropertyName("Released")] public DateOnly ReleaseDate { get; set; } //use dateonly?
    [JsonPropertyName("Poster")] public required string PosterUrl { get; set; }
    public List<Review> Reviews { get; set; } = [];
}