using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByIdsAsync(List<int> ids);
    }
}
