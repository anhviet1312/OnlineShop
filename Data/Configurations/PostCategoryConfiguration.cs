using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> entity)
        {
            entity.ToTable("PostCategories"); 

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Alias).HasMaxLength(256).IsRequired().HasColumnType("varchar");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ParentID);
            entity.Property(e => e.DisplayOrder);
            entity.Property(e => e.Image).HasMaxLength(256);
            entity.Property(e => e.HomeFlag);

            entity.HasMany(e => e.Posts)
                .WithOne(e => e.PostCategory)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
