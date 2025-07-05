using Domain.Common;

namespace Domain.Entities
{
    public class Favorite : BaseEntity
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
