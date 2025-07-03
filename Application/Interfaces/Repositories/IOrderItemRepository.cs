using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
    }
}
