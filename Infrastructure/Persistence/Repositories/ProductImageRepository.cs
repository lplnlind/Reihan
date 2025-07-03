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

        public async Task AddRangeAsync(IEnumerable<ProductImage> images)
        {
            await _dbSet.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }
    }
}
