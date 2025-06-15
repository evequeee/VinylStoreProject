using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectVinylStore.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVinylStore.DataAccess.Configurations
{
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
