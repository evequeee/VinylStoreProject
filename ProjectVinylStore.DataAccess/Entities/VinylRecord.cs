using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectVinylStore.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVinylStore.DataAccess.Entities
{
    public class VinylRecord
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;
    }

    public class VinylRecordConfiguration : IEntityTypeConfiguration<VinylRecord>
    {
        public void Configure(EntityTypeBuilder<VinylRecord> builder)
        {
            builder.ToTable("VinylRecords");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Title).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Artist).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Price).HasColumnType("decimal(10,2)");
        }
    }
}