using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet
                .Where(w => w.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<OrderItem>> GetByOrderIdsAsync(List<int> orderIds)
        {
            return await _dbSet
                .Where(w => orderIds.Contains(w.OrderId))
                .ToListAsync();
        }

        public async Task<bool> IsImageUsedAsync(string imageUrl)
            => await _dbSet.AnyAsync(o => o.ProductImage == imageUrl);
    }
}
