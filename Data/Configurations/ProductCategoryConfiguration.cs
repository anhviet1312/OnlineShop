using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories"); 

            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(256).IsRequired();
            builder.Property(e => e.Alias).HasMaxLength(256).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.ParentID);
            builder.Property(e => e.DisplayOrder);
            builder.Property(e => e.Image).HasMaxLength(256);
            builder.Property(e => e.HomeFlag);

            
            builder.HasMany(e => e.Products)
                .WithOne(e => e.ProductCategory)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
