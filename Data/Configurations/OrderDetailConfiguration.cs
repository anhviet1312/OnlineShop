using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnline.Models;

namespace ShopOnline.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> entity)
        {
            entity.ToTable("OrderDetails"); 

            entity.HasKey(e => new { e.OrderID, e.ProductID });

            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Price).IsRequired();

            
            entity.HasOne(e => e.Order)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(e => e.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            
            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
