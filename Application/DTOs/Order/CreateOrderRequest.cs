namespace Application.DTOs
{
    public class CreateOrderRequest
    {
        public AddressDto ShippingAddress { get; set; } = default!;
        public List<CartItemDto> CartItems { get; set; } = new();
    }
}
