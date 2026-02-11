namespace tedMovieApp.Dtos;

public class ReviewDto
{
    public string Title { get; set; }
    public string ReviewText { get; set; }
    public int Stars { get; set; }
    public int MovieId { get; set; }
}