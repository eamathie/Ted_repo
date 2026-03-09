using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace tedMovieApp.Models;

public class Review
{
    public int ReviewId { get; set; }

    public required string Title { get; set; }

    public required string UserId { get; set; } // foreign key to Identity User
    public IdentityUser? User { get; set; }
    public required string ReviewText { get; set; }
    [Range(1, 5)] public int Stars { get; set; }
    public int MovieId { get; set; }
}