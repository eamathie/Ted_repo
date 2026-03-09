using System.Text.Json.Serialization;

namespace tedMovieApp.Models;

public class Movie
{
    public int MovieId { get; set; }
    [JsonPropertyName("imdbID")] public string ImdbId  { get; set; }
    [JsonPropertyName("Title")] public string Title { get; set; }
    [JsonPropertyName("Genre")] public string Genre { get; set; }
    [JsonPropertyName("Runtime")] public string Length { get; set; }
    [JsonPropertyName("Plot")] public string Description { get; set; }
    [JsonPropertyName("Released")] public DateOnly ReleaseDate { get; set; } //use dateonly?
    [JsonPropertyName("Poster")] public string PosterUrl { get; set; }
    public List<Review> Reviews { get; set; } = [];
}