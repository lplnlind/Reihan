using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IProductClient
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
    }
}
