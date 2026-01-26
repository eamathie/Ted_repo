namespace tedMovieApp;

public class Movie
{
    public Guid MovieId { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public double Length { get; set; }
    public string Description { get; set; }
    public DateOnly ReleaseDate { get; set; } //use dateonly?
}