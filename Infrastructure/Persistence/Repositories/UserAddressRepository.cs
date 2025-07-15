using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserAddressRepository : RepositoryBase<UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<UserAddress>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task UpdateRangeAsync(List<UserAddress> addresses)
        {
            _dbSet.UpdateRange(addresses);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId)
            => await _dbSet.AnyAsync(f => f.UserId == userId);

        public async Task<UserAddress?> GetDefaultAddressAsync(int userId)
            => await _dbSet.FirstOrDefaultAsync(w => w.UserId == userId && w.IsDefault == true);
    }
}
