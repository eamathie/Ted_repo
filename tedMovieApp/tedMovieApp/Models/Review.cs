using System.ComponentModel.DataAnnotations;

namespace tedMovieApp;

public class Review
{
    public Guid ReviewId { get; set; }
    public string Title { get; set; }
    public string Movie  { get; set; }
    public string ReviewText { get; set; }
    [Range(1, 5)]
    public int Stars {get; set;}
}