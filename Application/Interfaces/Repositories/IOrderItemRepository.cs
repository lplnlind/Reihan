using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<List<OrderItem>> GetByOrderIdsAsync(List<int> orderIds);
        Task<bool> IsImageUsedAsync(string imageUrl);
    }
}
