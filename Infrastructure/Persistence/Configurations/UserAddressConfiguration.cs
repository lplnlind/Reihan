using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasOne(a => a.User)
                   .WithMany(u => u.Addresses)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Street).HasMaxLength(250);
            builder.Property(a => a.City).HasMaxLength(100);
            builder.Property(a => a.ZipCode).HasMaxLength(20);
        }
    }
}
