using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL; 

namespace ProjectVinylStore.DataAccess
{


    public class AppDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectVinylStoreData;Username=postgres;Password=1488;TrustServerCertificate=True;");
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VinylRecord> VinylRecords { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<VinylRecord>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Album>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Order>()
                .HasOne<User>(o => o.Users)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
