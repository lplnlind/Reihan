using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        Task<List<UserAddress>> GetByUserIdAsync(int userId);
        Task UpdateRangeAsync(List<UserAddress> addresses);
        Task<bool> ExistsAsync(int userId);
        Task<UserAddress?> GetDefaultAddressAsync(int userId);
    }
}
