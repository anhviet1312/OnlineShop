using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.ToTable("ProductTags"); 

            entity.HasKey(e => new { e.ProductID, e.TagID });
            entity.Property(e => e.ProductID);
            entity.Property(e => e.TagID).HasColumnType("varchar").HasMaxLength(50);

           
            entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductTags)
                .HasForeignKey(e => e.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            
            entity.HasOne(e => e.Tag)
                .WithMany()
                .HasForeignKey(e => e.TagID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
