using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> entity)
        {
            entity.ToTable("PostTags"); 

            entity.HasKey(e => new { e.PostID, e.TagID });
            entity.Property(e => e.PostID);
            entity.Property(e => e.TagID).HasColumnType("varchar").HasMaxLength(50);

            
            entity.HasOne(e => e.Post)
                .WithMany(e => e.PostTags)
                .HasForeignKey(e => e.PostID)
                .OnDelete(DeleteBehavior.Cascade);

            
            entity.HasOne(e => e.Tag)
                .WithMany()
                .HasForeignKey(e => e.TagID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
