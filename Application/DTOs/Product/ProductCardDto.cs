namespace Application.DTOs
{
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public bool IsInCart { get; set; } = false;

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
