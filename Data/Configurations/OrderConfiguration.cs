using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.ToTable("Orders"); 

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.CustomerName).HasMaxLength(256).IsRequired();
            entity.Property(e => e.CustomerAddress).HasMaxLength(256).IsRequired();
            entity.Property(e => e.CustomerEmail).HasMaxLength(256).IsRequired();
            entity.Property(e => e.CustomerMobile).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CustomerMessage).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PaymentMethod).HasMaxLength(256);
            entity.Property(e => e.CreatedDate);
            entity.Property(e => e.CreatedBy);
            entity.Property(e => e.PaymentStatus);
            entity.Property(e => e.Status);
            entity.Property(e => e.CustomerId).HasMaxLength(450).HasColumnType("nvarchar");

            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            
            entity.HasMany(e => e.OrderDetails)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
