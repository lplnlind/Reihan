using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.ImageUrl ?? string.Empty
            };

            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.Id);
            if (product is null) return;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.ImageUrl = dto.ImageUrl ?? string.Empty;

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is not null)
                await _productRepository.DeleteAsync(product.Id);
        }
    }
}
