using Application.DTOs;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int Id, string newStatus);
        Task<List<OrderDetailsDto>> GetOrdersByUserAsync(int userId);
        Task<int> CreateOrderAsync(int userId, CreateOrderRequest request);
        Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId);
    }
}
