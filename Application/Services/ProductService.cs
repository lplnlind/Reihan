using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, 
            IProductImageRepository productImageRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productImages = await _productImageRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = categoryDict.GetValueOrDefault(p.CategoryId),
                ImageUrls = productImages.Where(w => w.ProductId == p.Id).Select(s => s.Url).ToList()
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;

            var productImages = await _productImageRepository.GetByProductIdAsync(product.Id);
            var category = await _categoryRepository.GetByIdAsync(product.CategoryId);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity= product.StockQuantity,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = category?.Name,
                ImageUrls = productImages.Select(url => url.Url).ToList()
            };
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                StockQuantity = dto.StockQuantity,
                Description = dto.Description,
            };

            await _productRepository.AddAsync(product);

            if (dto.ImageUrls is not null && dto.ImageUrls.Any())
            {
                var images = dto.ImageUrls.Select(url => new ProductImage
                {
                    ProductId = product.Id, 
                    Url = url
                }).ToList();

                await _productImageRepository.AddRangeAsync(images);
            }
        }

        public async Task UpdateProductAsync(ProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.Id);
            if (product is null) return;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.StockQuantity = dto.StockQuantity;
            product.Description = dto.Description;

            await _productRepository.UpdateAsync(product);

            if (dto.ImageUrls is not null && dto.ImageUrls.Any())
            {
                var images = dto.ImageUrls.Select(url => new ProductImage
                {
                    ProductId = product.Id,
                    Url = url
                }).ToList();

                await _productImageRepository.AddRangeAsync(images);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return;

            var images = await _productImageRepository.GetByProductIdAsync(id);

            foreach (var image in images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            await _productRepository.DeleteAsync(product.Id);
        }

        public async Task<List<ProductDto>> GetLatestProductsAsync(int count = 8)
        {
            var products = await _productRepository.GetAllAsync();
            var images = await _productImageRepository.GetAllAsync();

            var latest = products
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToList();

            return latest.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                ImageUrls = images.Where(i => i.ProductId == p.Id).Select(i => i.Url).ToList()
            }).ToList();
        }

        public async Task<List<ProductDto>> GetBestSellingProductsAsync(int count = 8)
        {
            var products = await _productRepository.GetAllAsync();
            return new(); 
        }

        public async Task<List<ProductDto>> GetFilteredProductsAsync(int? categoryId = null)
        {
            var products = await _productRepository.GetAllAsync();
            var images = await _productImageRepository.GetAllAsync();

            if (categoryId is not null)
                products = products.Where(p => p.CategoryId == categoryId).ToList();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                ImageUrls = images.Where(i => i.ProductId == p.Id).Select(i => i.Url).ToList()
            }).ToList();
        }

    }
}
