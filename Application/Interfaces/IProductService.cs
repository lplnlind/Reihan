using Reihan.Shared.DTOs;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(bool includeInactive = false);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDto product);
        Task UpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(int id);
        Task<List<ProductDto>> GetLatestProductsAsync(int count = 8);
        Task<List<ProductDto>> GetBestSellingProductsAsync(int count = 8);
        Task<List<ProductDto>> GetFilteredProductsAsync(int? categoryId = null);
        Task<List<ProductDto>> GetSpecialSalesAsync(int count = 2);
        Task<bool> IsInCartAsync(int userId, int productId);
        Task SetActiveStatusAsync(int productId, bool isActive);
    }
}
