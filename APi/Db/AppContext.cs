using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Tourplanner.Entities;
using Tourplanner.Entities.Tours;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Models;

namespace Tourplanner;

public class TourContext : DbContext
{
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourLog> TourLogs { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<Tile> Tiles { get; set; }

    public TourContext(DbContextOptions<TourContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.From)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.To)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.Distance);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            entity.Property(e => e.EstimatedTime);
            entity.Property(e => e.Popularity);
            entity.Property(e => e.ChildFriendliness);
            entity.HasOne(e => e.Map)
                .WithOne(m => m.Tour)
                .HasForeignKey<Map>(e => e.TourId);
            entity.OwnsOne(e => e.Coordinates, navigationBuilder =>
            {
                navigationBuilder.Property(c => c.Longitude).HasColumnName("Coordinates_Longitude");
                navigationBuilder.Property(c => c.Latitude).HasColumnName("Coordinates_Latitude");
            });
            entity.HasMany<TourLog>()
                .WithOne(e => e.Tour)
                .HasForeignKey(e => e.TourId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TourLog>(entity =>
        {
            entity.HasKey(e => e.TourLogId);
            entity.Property(e => e.Difficulty);
            entity.Property(e => e.Duration);
            entity.Property(e => e.Rating);
            entity.Property(e => e.DateTime);
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.HasOne(e => e.Tour)
                .WithMany(e => e.TourLogs)
                .HasForeignKey(e => e.TourId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Map>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Zoom);
                entity.OwnsOne<Bbox>(e => e.Bbox);
                entity.HasOne(e => e.Tour)
                    .WithOne(e => e.Map)
                    .HasForeignKey<Tour>(e => e.MapId);
                entity.HasMany(e => e.Tiles)
                    .WithOne(e => e.Map)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        modelBuilder.Entity<Tile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Zoom);
            entity.Property(e => e.X);
            entity.Property(e => e.Y);
            entity.Property(e => e.Order);
            entity.Ignore(e => e.Base64Encoded);
        });


        base.OnModelCreating(modelBuilder);
    }
}