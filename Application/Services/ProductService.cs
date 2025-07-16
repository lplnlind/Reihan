using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _productImageRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo,
            IProductImageRepository productImageRepo,
            ICategoryRepository categoryRepo,
            ICartRepository cartRepo,
            IUserContextService userContextService,
            IOrderItemRepository orderItemRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _categoryRepo = categoryRepo;
            _cartRepo = cartRepo;
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var productImages = await _productImageRepo.GetAllAsync();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var productsDto = _mapper.Map<List<ProductDto>>(products);

            foreach (var p in productsDto)
            {
                p.CategoryName = categoryDict.GetValueOrDefault(p.CategoryId);
                p.ImageUrl = productImages.FirstOrDefault(w => w.ProductId == p.Id)?.Url ?? string.Empty;
                p.ImageUrls = productImages.Where(w => w.ProductId == p.Id).Select(i => i.Url).ToList();
            }

            return productsDto;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null) return null;

            var productImages = await _productImageRepo.GetByProductIdAsync(product.Id);
            var category = await _categoryRepo.GetByIdAsync(product.CategoryId);

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = category?.Name;
            productDto.ImageUrls = productImages.Where(w => w.ProductId == productDto.Id).Select(s => s.Url).ToList() ?? new List<string>();

            return productDto;
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

            // مدیریت تصاویر
            var existingImages = await _productImageRepo.GetByProductIdAsync(product.Id);
            var existingUrls = existingImages.Select(i => i.Url).ToList();
            var newUrls = dto.ImageUrls ?? new List<string>();

            // 🔴 حذف عکس‌هایی که در لیست جدید نیستند
            var toRemove = existingImages.Where(i => !newUrls.Contains(i.Url)).ToList();
            if (toRemove.Any())
            {
                foreach (var image in toRemove)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));

                    bool isUsedInOrder = await _orderItemRepo.IsImageUsedAsync(image.Url);
                    if (isUsedInOrder) continue;

                    if (File.Exists(path))
                        File.Delete(path);
                }

                await _productImageRepo.DeleteRangeAsync(toRemove);
            }

            // 🟢 افزودن عکس‌های جدید که قبلاً وجود نداشتند
            var toAdd = newUrls.Except(existingUrls).Select(url => new ProductImage
            {
                ProductId = product.Id,
                Url = url
            }).ToList();

            if (toAdd.Any())
                await _productImageRepo.AddRangeAsync(toAdd);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null) return;

            var images = await _productImageRepo.GetByProductIdAsync(id);

            foreach (var image in images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));

                bool isUsedInOrder = await _orderItemRepo.IsImageUsedAsync(image.Url);
                if (isUsedInOrder) continue;

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            await _productRepo.DeleteAsync(product.Id);
        }

        public async Task<List<ProductDto>> GetLatestProductsAsync(int count = 8)
        {
            var products = await _productRepo.GetAllAsync();
            var images = await _productImageRepo.GetAllAsync();

            var latest = products
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToList();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);


            return latest.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = categoryDict.GetValueOrDefault(p.CategoryId),
                ImageUrl = images.Where(w => w.ProductId == p.Id).Select(s => s.Url)?.FirstOrDefault() ?? string.Empty,
            }).ToList();
        }

        public async Task<List<ProductDto>> GetBestSellingProductsAsync(int count = 8)
        {
            var products = await _productRepo.GetAllAsync();
            return new();
        }

        public async Task<List<ProductDto>> GetFilteredProductsAsync(int? categoryId = null)
        {
            var products = await _productRepo.GetAllAsync();
            var images = await _productImageRepo.GetAllAsync();

            if (categoryId is not null)
                products = products.Where(p => p.CategoryId == categoryId).ToList();

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
                ImageUrl = images.Where(w => w.ProductId == p.Id).Select(s => s.Url)?.FirstOrDefault() ?? string.Empty,
            }).ToList();
        }

        public async Task<bool> IsInCartAsync(int userId, int productId)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);
            return cart.Items.Select(i => i.ProductId).ToHashSet().Contains(productId);
        }
    }
}
