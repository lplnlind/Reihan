namespace Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}
