using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> entity) {
            entity.ToTable("Tags"); 

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnType("varchar").HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Type).HasMaxLength(50).IsRequired();
        }
    }
}
