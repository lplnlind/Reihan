using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Role)
                .IsRequired();

            // Email به‌عنوان Owned Type
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(100)
                    .IsRequired();

                email.HasIndex(e => e.Value)
                    .IsUnique();
            });

            // Address به‌عنوان Owned Type
            builder.OwnsOne(u => u.Address, address =>
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

            // ارتباط با Orders از طریق Navigation Property خودش در User تعریف شده
            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
