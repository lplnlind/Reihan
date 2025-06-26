using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(int userId);
    }
}
