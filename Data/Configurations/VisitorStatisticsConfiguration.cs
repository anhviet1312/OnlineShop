using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class VisitorStatisticsConfiguration : IEntityTypeConfiguration<VisitorStatistics>
    {
        public void Configure(EntityTypeBuilder<VisitorStatistics> entity)
        {
            entity.ToTable("VisitorStatistics"); 

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).ValueGeneratedNever(); 

            entity.Property(e => e.VisitedDate).IsRequired();
            entity.Property(e => e.IPAddress).HasMaxLength(50);
        }
    }
}
