using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VinylRecord> VinylRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<VinylRecord>().HasData(
                new VinylRecord
                {
                    Id = 1,
                    Title = "Ride The Lightning",
                    Artist = "Metallica",
                    Genre = "Thrash-Metal",
                    ReleaseDate = new DateTime(1984, 1, 1),
                    Price = 39.99m,
                    StockQuantity = 100,
                    CoverImageUrl = "https://example.com/ride_the_lightning.jpg"
                },
                new VinylRecord
                {
                    Id = 2,
                    Title = "example",
                    Artist = "example",
                    Genre = "example",
                    ReleaseDate = new DateTime(1, 1, 1),
                    Price = 1.1m,
                    StockQuantity = 100,
                    CoverImageUrl = "https://example.com/example.jpg"
                }
            );
        }
    }
}
