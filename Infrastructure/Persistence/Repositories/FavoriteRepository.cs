using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class FavoriteRepository : RepositoryBase<Favorite>, IFavoriteRepository
    {

        public FavoriteRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Favorite>> GetByUserIdAsync(int userId)
            => await _dbSet.Where(f => f.UserId == userId).ToListAsync();

        public async Task<bool> ExistsAsync(int userId, int productId)
            => await _dbSet.AnyAsync(f => f.UserId == userId && f.ProductId == productId);

        public async Task RemoveByUserAndProductAsync(int userId, int productId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
