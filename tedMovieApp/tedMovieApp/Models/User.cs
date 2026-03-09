namespace tedMovieApp.Models;

public class User
{
    public int UserId { get; set; }
    public required string Name { get; set; }
    public List<Review>? Reviews { get; set; }
    
}