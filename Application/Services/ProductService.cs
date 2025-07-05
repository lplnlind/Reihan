using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _productImageRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICartRepository _cartRepo;
        private IUserContextService _userContextService;

        public ProductService(IProductRepository productRepo, 
            IProductImageRepository productImageRepo,
            ICategoryRepository categoryRepo,
            ICartRepository cartRepo,
            IUserContextService userContextService)
        {
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _categoryRepo = categoryRepo;
            _cartRepo = cartRepo;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var productImages = await _productImageRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
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
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null) return null;

            var productImages = await _productImageRepo.GetByProductIdAsync(product.Id);
            var category = await _categoryRepo.GetByIdAsync(product.CategoryId);

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

            await _productRepo.AddAsync(product);

            if (dto.ImageUrls is not null && dto.ImageUrls.Any())
            {
                var images = dto.ImageUrls.Select(url => new ProductImage
                {
                    ProductId = product.Id, 
                    Url = url
                }).ToList();

                await _productImageRepo.AddRangeAsync(images);
            }
        }

        public async Task UpdateProductAsync(ProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.Id);
            if (product is null) return;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.StockQuantity = dto.StockQuantity;
            product.Description = dto.Description;

            await _productRepo.UpdateAsync(product);

            if (dto.ImageUrls is not null && dto.ImageUrls.Any())
            {
                var images = dto.ImageUrls.Select(url => new ProductImage
                {
                    ProductId = product.Id,
                    Url = url
                }).ToList();

                await _productImageRepo.AddRangeAsync(images);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null) return;

            var images = await _productImageRepo.GetByProductIdAsync(id);

            foreach (var image in images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            await _productRepo.DeleteAsync(product.Id);
        }

        public async Task<List<ProductCardDto>> GetLatestProductsAsync(int count = 8)
        {
            var products = await _productRepo.GetAllAsync();
            var images = await _productImageRepo.GetAllAsync();

            var latest = products
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToList();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var userId = _userContextService.GetUserId();
            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);
            var productIdsInCart = cart.Items.Select(i => i.ProductId).ToHashSet();


            return products.Select(p => new ProductCardDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = categoryDict.GetValueOrDefault(p.CategoryId),
                ImageUrl = images.Where(w => w.ProductId == p.Id).Select(s => s.Url)?.FirstOrDefault() ?? string.Empty,
                IsInCart = productIdsInCart.Contains(p.Id)
            }).ToList();
        }

        public async Task<List<ProductDto>> GetBestSellingProductsAsync(int count = 8)
        {
            var products = await _productRepo.GetAllAsync();
            return new(); 
        }

        public async Task<List<ProductCardDto>> GetFilteredProductsAsync(int? categoryId = null)
        {
            var products = await _productRepo.GetAllAsync();
            var images = await _productImageRepo.GetAllAsync();

            if (categoryId is not null)
                products = products.Where(p => p.CategoryId == categoryId).ToList();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var userId = _userContextService.GetUserId();
            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);
            var productIdsInCart = cart.Items.Select(i => i.ProductId).ToHashSet();


            return products.Select(p => new ProductCardDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = categoryDict.GetValueOrDefault(p.CategoryId),
                ImageUrl = images.Where(w => w.ProductId == p.Id).Select(s => s.Url)?.FirstOrDefault() ?? string.Empty,
                IsInCart = productIdsInCart.Contains(p.Id)
            }).ToList();
        }

        public async Task<IEnumerable<ProductCardDto>> GetProductCardAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var productImages = await _productImageRepo.GetAllAsync();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var userId = _userContextService.GetUserId();
            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);
            var productIdsInCart = cart.Items.Select(i => i.ProductId).ToHashSet();


            return products.Select(p => new ProductCardDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = categoryDict.GetValueOrDefault(p.CategoryId),
                ImageUrl = productImages.Where(w => w.ProductId == p.Id).Select(s => s.Url)?.FirstOrDefault() ?? string.Empty,
                IsInCart = productIdsInCart.Contains(p.Id)
            });
        }
    }
}
