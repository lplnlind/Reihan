using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsMany(c => c.Items, items =>
            {
                items.WithOwner().HasForeignKey("CartId");
                items.Property<int>("Id"); // EF نیاز به کلید دارد
                items.HasKey("Id");
            });
        }
    }
}
