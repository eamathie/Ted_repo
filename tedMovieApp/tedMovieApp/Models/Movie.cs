namespace tedMovieApp;

public class Movie
{
    public int MovieId { get; set; }
    public string ImdbId  { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Length { get; set; }
    public string Description { get; set; }
    public DateOnly ReleaseDate { get; set; } //use dateonly?
    public string PosterUrl { get; set; }
    public List<Review> Reviews { get; set; } = [];
}