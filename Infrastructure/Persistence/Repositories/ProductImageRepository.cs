using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
        {
            return await _dbSet.AsNoTracking().Where(w => w.ProductId == productId).ToListAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetByProductIdsAsync(List<int> ids)
        {
            return await _dbSet
                .Where(w => ids.Contains(w.ProductId))
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<ProductImage> images)
        {
            await _dbSet.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByProductIdAsync(int productId)
        {
            var images = await _dbSet.Where(w => w.ProductId == productId).ToListAsync();
            if (images.Any())
            {
                _dbSet.RemoveRange(images);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<ProductImage> images)
        {
            _dbSet.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

    }
}
