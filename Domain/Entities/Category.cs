using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // ارتباط یک به چند با محصولات
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
