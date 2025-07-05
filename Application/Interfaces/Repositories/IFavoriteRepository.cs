using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        Task<List<Favorite>> GetByUserIdAsync(int userId);
        Task<bool> ExistsAsync(int userId, int productId);
        Task RemoveByUserAndProductAsync(int userId, int productId);
    }
}
