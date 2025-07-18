using Reihan.Client.Enums;
using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IOrderClient
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<List<OrderDetailsDto>> GetOrderDetailsAsync(int id);
        Task UpdateOrderStatusAsync(int id, OrderStatus newStatus);
        Task<List<OrderDetailsDto>> GetOrdersByUserAsync();
        Task<int> CreateAsync(CreateOrderRequest request);
    }
}
