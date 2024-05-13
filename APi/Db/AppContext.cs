﻿using Microsoft.EntityFrameworkCore;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;
using Tourplanner.Models;

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
                    EstimatedTime = 2.0f,
                    ImagePath = "example1.jpg",
                    Popularity = 4.5f,
                    ChildFriendliness = 4.0f,
                    TransportType = TransportType.Car
                }
            );
        modelBuilder.Entity<TourLog>()
            .HasData(
                new TourLog
                {
                    TourLogId = 1,
                    Difficulty = 3.5f,
                    Duration = 2.0f,
                    Rating = 4.0f,
                    Comment = "This was a great tour!",
                    TourId = 1
                });

        modelBuilder.Entity<Tour>()
            .Property(t => t.TransportType)
            .HasConversion<int>();
        modelBuilder.Entity<Tour>()
            .HasMany<TourLog>(t => t.TourLogs)
            .WithOne()
            .HasForeignKey(to => to.TourId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=192.168.0.130;Database=tourplanner;Username=tour_admin;Password=tour_admin123");
    }
}