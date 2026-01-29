using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace tedMovieApp;

public class MovieReviewApiContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public MovieReviewApiContext(DbContextOptions<MovieReviewApiContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {  
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.MovieId)
            .IsUnique();
        
        modelBuilder.Entity<Review>()
            .HasIndex(r => r.ReviewId)
            .IsUnique();
    }
    
}