using Reihan.Client.Models.Cart;

namespace Reihan.Client.Models.Order
{
    public class CreateOrderRequest
    {
        public AddressDto ShippingAddress { get; set; } = default!;
        public List<CartItemDto> CartItems { get; set; } = new();
    }
}
