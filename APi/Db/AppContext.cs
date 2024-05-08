using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;

namespace Tourplanner;

public class TourContext : DbContext
{
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourLog> TourLogs { get; set; }
    
    public TourContext(DbContextOptions<TourContext> options)
        : base(options) {}

    //
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Tours>()
    //         .Property()
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=192.168.0.130;Database=tourplanner;Username=tour_admin;Password=tour_admin123");
    }
}