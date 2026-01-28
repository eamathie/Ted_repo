using System.ComponentModel.DataAnnotations;

namespace tedMovieApp;

public class Review
{
    public int ReviewId { get; set; }
    public string Title { get; set; }
    //public Movie Movie { get; set; }
    public string ReviewText { get; set; }
    [Range(1, 5)]
    public int Stars {get; set;}
}