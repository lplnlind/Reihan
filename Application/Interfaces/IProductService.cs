using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDto product);
        Task UpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(int id);
        Task<List<ProductDto>> GetLatestProductsAsync(int count = 8);
        Task<List<ProductDto>> GetBestSellingProductsAsync(int count = 8);
        Task<List<ProductDto>> GetFilteredProductsAsync(int? categoryId = null);

    }
}
