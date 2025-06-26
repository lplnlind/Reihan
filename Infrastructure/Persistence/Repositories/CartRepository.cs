using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Cart?> GetByUserIdAsync(int userId)
            => await _dbSet.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
    }
}
