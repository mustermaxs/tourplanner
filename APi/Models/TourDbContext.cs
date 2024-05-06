using Microsoft.EntityFrameworkCore;

namespace Tourplanner.Models;
public class TourDbContext : DbContext {
public TourDbContext(DbContextOptions<TourDbContext> options)
: base(options) { }
public DbSet<Tour> Products => Set<Product>();
}
