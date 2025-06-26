using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(a => a.Street)
                    .HasMaxLength(200)
                    .IsRequired();

                address.Property(a => a.City)
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(a => a.ZipCode)
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }
    }
}
