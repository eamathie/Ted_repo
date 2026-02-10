namespace tedMovieApp.Dtos;

public class UpdateReviewDto
{
    public string Title { get; set; }
    public string ReviewText { get; set; }
    public int Stars { get; set; }
    public int MovieId { get; set; } // Foreign key
}