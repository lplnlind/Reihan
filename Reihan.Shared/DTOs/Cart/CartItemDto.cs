namespace Reihan.Shared.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FinalPrice { get; set; }

        public decimal Total => FinalPrice * Quantity;
    }
}
