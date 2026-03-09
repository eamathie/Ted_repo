namespace tedMovieApp.Models.Dtos;

public class ReviewDto
{
    public required string Title { get; set; }
    public required string ReviewText { get; set; }
    public int Stars { get; set; }
}