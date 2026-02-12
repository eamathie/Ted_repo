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

        modelBuilder.Entity<Review>().HasOne(r => r.User)
            .WithMany(); // or .WithMany(u => u.Reviews) if you add a collection .HasForeignKey(r => r.UserId) .OnDelete(DeleteBehavior.Cascade);
    }
    
}