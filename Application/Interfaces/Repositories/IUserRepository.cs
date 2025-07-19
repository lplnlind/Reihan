using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByIdsAsync(List<int> ids);
        Task<bool> IsUserActiveAsync(int userId);
    }
}
