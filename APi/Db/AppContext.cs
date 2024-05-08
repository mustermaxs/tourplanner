using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;

namespace Tourplanner;

public class TourContext : DbContext
{
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourLog> TourLogs { get; set; }

    public TourContext(DbContextOptions<TourContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>()
            .HasData(
                new
                {
                    TourId = 1,
                    Name = "Example Tour 1",
                    From = "Origin 1",
                    To = "Destination 1",
                    Distance = 100.0f,
                    Description = "This is an example tour 1.",
                    EstimatedTime = TimeSpan.FromHours(2),
                    ImagePath = "example1.jpg",
                    Popularity = 4.5f,
                    ChildFriendliness = 4.0f
                }
            );
        modelBuilder.Entity<TourLog>()
            .HasData(
                new TourLog
                {
                    TourLogId = 1,
                    Difficulty = 3.5f,
                    Duration = TimeSpan.FromHours(1),
                    Rating = 4.0f,
                    Comment = "This was a great tour!",
                    TourId = 1
                });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=192.168.0.130;Database=tourplanner;Username=tour_admin;Password=tour_admin123");
    }
}