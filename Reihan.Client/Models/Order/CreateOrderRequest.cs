using Reihan.Client.Models;

namespace Reihan.Client.Models
{
    public class CreateOrderRequest
    {
        public AddressDto ShippingAddress { get; set; } = new();
        public List<CartItemDto> CartItems { get; set; } = new();
    }
}
