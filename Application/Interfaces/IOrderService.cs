using Domain.Enums;
using Reihan.Shared.DTOs;

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
