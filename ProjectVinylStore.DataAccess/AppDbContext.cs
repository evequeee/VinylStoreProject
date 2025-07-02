using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess
{
    public class AppDbContext : IdentityDbContext <ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VinylRecord> VinylRecords { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important for Identity

            modelBuilder.Entity<VinylRecord>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Album>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Order>()
                .HasOne<ApplicationUser>(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VinylRecord>()
                .HasOne(v => v.Album)
                .WithMany()
                .HasForeignKey(v => v.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
