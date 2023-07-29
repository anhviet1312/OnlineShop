using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            
                builder.HasKey(e => e.ID);
                builder.Property(e => e.ID).ValueGeneratedOnAdd();
                builder.Property(e => e.Name).HasMaxLength(256).IsRequired();
                builder.Property(e => e.Alias).HasMaxLength(256).IsRequired();
                builder.Property(e => e.CategoryID).IsRequired();
                builder.Property(e => e.Image).HasMaxLength(256);
                builder.Property(e => e.MoreImages).HasColumnType("xml");
                builder.Property(e => e.Price).IsRequired();
                builder.Property(e => e.PromotionPrice);
                builder.Property(e => e.Description).HasMaxLength(500);
                builder.Property(e => e.Content);
                builder.Property(e => e.HomeFlag);
                builder.Property(e => e.HotFlag);
                builder.Property(e => e.ViewCount);
                builder.Property(e => e.Tags);
                builder.Property(e => e.Quantity);
                builder.Property(e => e.OriginalPrice);

                builder.HasOne(e => e.ProductCategory)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);

                
                builder.HasMany(e => e.ProductTags)
                    .WithOne(e => e.Product)
                    .HasForeignKey(e => e.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}
