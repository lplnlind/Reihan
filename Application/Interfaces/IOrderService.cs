using Reihan.Shared.DTOs;
using Reihan.Shared.Enums;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int Id, OrderStatus newStatus);
        Task<List<OrderDetailsDto>> GetOrdersByUserAsync(int userId);
        Task<int> CreateOrderAsync(int userId, CreateOrderRequest request);
        Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId);
    }
}
