using Application.DTOs;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(int userId, CreateOrderRequest request);
        Task<List<OrderDto>> GetOrdersByUserAsync(int userId);
    }
}
