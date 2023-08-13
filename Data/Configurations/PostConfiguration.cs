using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> entity)
        {
            entity.ToTable("Posts"); // Explicitly set the table name

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Alias).HasMaxLength(256).IsRequired().HasColumnType("varchar");
            entity.Property(e => e.CategoryID).IsRequired();
            entity.Property(e => e.Image).HasMaxLength(256);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Content);
            entity.Property(e => e.HomeFlag);
            entity.Property(e => e.HotFlag);
            entity.Property(e => e.ViewCount);

            
            entity.HasOne(e => e.PostCategory)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

           
            entity.HasMany(e => e.PostTags)
                .WithOne(e => e.Post)
                .HasForeignKey(e => e.PostID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
