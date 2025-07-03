using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IOrderClient
    {
        Task<List<OrderDto>> GetAllAsync();
        Task UpdateOrderStatusAsync(int id, string newStatus);
        Task<List<OrderDto>> GetOrdersByUserAsync();
        Task<int> CreateAsync(CreateOrderRequest request);
    }
}
