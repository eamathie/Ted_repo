using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace tedMovieApp;

public class Review
{
    public int ReviewId { get; set; }

    public string Title { get; set; }

    public string? UserId { get; set; } // foreign key to Identity User
    public IdentityUser? User { get; set; }
    public string ReviewText { get; set; }
    [Range(1, 5)] public int Stars { get; set; }
    public int MovieId { get; set; }
}