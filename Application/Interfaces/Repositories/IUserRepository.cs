using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
