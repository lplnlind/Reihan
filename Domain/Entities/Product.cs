using Domain.Common;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; private set; } = true;

        // ارتباط با Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<ProductImage> Images { get; set; } = new();
        public List<Favorite> Favorites { get; set; } = new();

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("تعداد باید بیشتر از صفر باشد.");

            if (quantity > StockQuantity)
                throw new InvalidOperationException("موجودی کافی نیست.");

            StockQuantity -= quantity;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("تعداد باید بیشتر از صفر باشد.");

            StockQuantity += quantity;
        }
    }
}
