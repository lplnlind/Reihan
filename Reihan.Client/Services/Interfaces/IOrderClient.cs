using Reihan.Shared.Enums;
using Reihan.Shared.DTOs;

namespace Reihan.Client.Services
{
    public interface IOrderClient
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDetailsDto> GetOrderDetailsAsync(int id);
        Task UpdateOrderStatusAsync(int id, SharedOrderStatus newStatus);
        Task<List<OrderDetailsDto>> GetOrdersByUserAsync();
        Task<int> CreateAsync(CreateOrderRequest request);
    }
}
