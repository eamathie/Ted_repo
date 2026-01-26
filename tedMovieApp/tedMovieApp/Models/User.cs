namespace tedMovieApp;

public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public List<Review> Reviews { get; set; }
    
}