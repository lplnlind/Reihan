using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepo,
            IProductImageRepository productImageRepo,
            ICategoryRepository categoryRepo,
            ICartRepository cartRepo,
            IOrderItemRepository orderItemRepo,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _categoryRepo = categoryRepo;
            _cartRepo = cartRepo;
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var images = await _productImageRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();

            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);
            var productsDto = _mapper.Map<List<ProductDto>>(products);

            foreach (var p in productsDto)
            {
                p.CategoryName = categoryDict.GetValueOrDefault(p.CategoryId);
                p.ImageUrl = images.FirstOrDefault(w => w.ProductId == p.Id)?.Url ?? string.Empty;
                p.ImageUrls = images.Where(w => w.ProductId == p.Id).Select(i => i.Url).ToList();
            }

            return productsDto;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
            {
                _logger.LogWarning("محصول با شناسه {Id} یافت نشد", id);
                throw new AppException("محصول یافت نشد",
                    StatusCodes.Status404NotFound,
                    ErrorCode.ProductNotFound);
            }

            var productImages = await _productImageRepo.GetByProductIdAsync(product.Id);
            var category = await _categoryRepo.GetByIdAsync(product.CategoryId);

            var dto = _mapper.Map<ProductDto>(product);
            dto.CategoryName = category?.Name;
            dto.ImageUrls = productImages.Select(s => s.Url).ToList();
            dto.ImageUrl = productImages.FirstOrDefault()?.Url ?? string.Empty;

            return dto;
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _productRepo.AddAsync(product);

            _logger.LogInformation("محصول جدید اضافه شد: {Name}", product.Name);

            if (dto.ImageUrls is not null && dto.ImageUrls.Any())
            {
                var images = dto.ImageUrls.Select(url => new ProductImage
                {
                    ProductId = product.Id,
                    Url = url
                }).ToList();

                await _productImageRepo.AddRangeAsync(images);
                _logger.LogInformation("تصاویر محصول {Name} اضافه شدند.", product.Name);
            }
        }

        public async Task UpdateProductAsync(ProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.Id);
            if (product is null)
            {
                _logger.LogWarning("محصول برای بروزرسانی یافت نشد: {Id}", dto.Id);
                throw new AppException("محصول یافت نشد", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.ProductNotFound);
            }

            product = _mapper.Map<Product>(dto);
            await _productRepo.UpdateAsync(product);

            // Update Product Images
            var existingImages = await _productImageRepo.GetByProductIdAsync(product.Id);
            var existingUrls = existingImages.Select(i => i.Url).ToList();
            var newUrls = dto.ImageUrls ?? new();

            var toRemove = existingImages.Where(i => !newUrls.Contains(i.Url)).ToList();
            foreach (var image in toRemove)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));
                if (await _orderItemRepo.IsImageUsedAsync(image.Url)) continue;

                if (File.Exists(path))
                    File.Delete(path);
            }

            if (toRemove.Any())
                await _productImageRepo.DeleteRangeAsync(toRemove);

            var toAdd = newUrls.Except(existingUrls).Select(url => new ProductImage
            {
                ProductId = product.Id,
                Url = url
            }).ToList();

            if (toAdd.Any())
                await _productImageRepo.AddRangeAsync(toAdd);

            _logger.LogInformation("محصول با موفقیت بروزرسانی شد: {Id}", product.Id);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
            {
                _logger.LogWarning("محصول برای حذف یافت نشد: {Id}", id);
                throw new AppException("محصول یافت نشد",
                    StatusCodes.Status404NotFound,
                    ErrorCode.ProductNotFound);
            }

            var images = await _productImageRepo.GetByProductIdAsync(id);
            foreach (var image in images)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));
                if (await _orderItemRepo.IsImageUsedAsync(image.Url)) continue;

                if (File.Exists(path))
                    File.Delete(path);
            }

            await _productRepo.DeleteAsync(product.Id);
            _logger.LogInformation("محصول با موفقیت حذف شد: {Id}", id);
        }

        public async Task<List<ProductDto>> GetLatestProductsAsync(int count = 8)
        {
            var products = await _productRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var latest = products.OrderByDescending(p => p.CreatedAt).Take(count).ToList();
            var latestIds = latest.Select(s => s.Id).ToList();
            var images = await _productImageRepo.GetByProductIdsAsync(latestIds);

            var productsDto = _mapper.Map<List<ProductDto>>(latest);
            foreach (var product in productsDto)
            {
                product.CategoryName = categoryDict.GetValueOrDefault(product.CategoryId);
                product.ImageUrl = images.FirstOrDefault(w => w.ProductId == product.Id)?.Url ?? string.Empty;
            }
            return productsDto;
        }

        public async Task<List<ProductDto>> GetBestSellingProductsAsync(int count = 8)
        {
            // در آینده پیاده‌سازی شود
            return new();
        }

        public async Task<List<ProductDto>> GetFilteredProductsAsync(int? categoryId = null)
        {
            var products = await _productRepo.GetAllAsync();
            var images = await _productImageRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            if (categoryId is not null)
                products = products.Where(p => p.CategoryId == categoryId).ToList();

            var productsDto = _mapper.Map<List<ProductDto>>(products);
            foreach (var product in productsDto)
            {
                product.CategoryName = categoryDict.GetValueOrDefault(product.CategoryId);
                product.ImageUrl = images.FirstOrDefault(w => w.ProductId == product.Id)?.Url ?? string.Empty;
            }
            return productsDto;
        }

        public async Task<bool> IsInCartAsync(int userId, int productId)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);
            return cart.Items.Any(i => i.ProductId == productId);
        }
    }
}