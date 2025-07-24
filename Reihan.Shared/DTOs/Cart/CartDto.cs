namespace Reihan.Shared.DTOs
{
    public class CartDto
    {
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(i => i.FinalPrice * i.Quantity);
        public decimal TotalWithoutDiscount => Items.Sum(i => i.UnitPrice * i.Quantity);
        public decimal TotalDiscount => TotalWithoutDiscount - TotalPrice;
    }
}
