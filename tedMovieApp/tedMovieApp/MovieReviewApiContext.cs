using Microsoft.EntityFrameworkCore;

namespace tedMovieApp;

public class MovieReviewApiContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tedmovieapp;User Id=postgres;Password=emilisverycool");
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.MovieId)
            .IsUnique();
        
        modelBuilder.Entity<Review>()
            .HasIndex(r => r.ReviewId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserId)
            .IsUnique();
    }
    
}