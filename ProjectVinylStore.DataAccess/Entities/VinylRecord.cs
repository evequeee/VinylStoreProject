﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ProjectVinylStore.DataAccess.Entities
{
    public class VinylRecord
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;

        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;
        public string Artist => Album?.Artist ?? string.Empty;
        public string Genre => Album?.Genre ?? string.Empty;
        public DateTime ReleaseDate => Album?.ReleaseDate ?? default;
    }

    public class VinylRecordConfiguration : IEntityTypeConfiguration<VinylRecord>
    {
        public void Configure(EntityTypeBuilder<VinylRecord> builder)
        {
            builder.ToTable("VinylRecords");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Title).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Price).HasColumnType("decimal(10,2)");
        }
    }
}