using Reihan.Client.Models.Order;

namespace Reihan.Client.Services
{
    public interface IOrderClient
    {
        Task<int> CreateOrderAsync(CreateOrderRequest request);
        Task<List<OrderDto>> GetOrdersAsync();
    }
}
