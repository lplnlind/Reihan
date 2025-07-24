using Reihan.Shared.DTOs;
using Application.Exceptions;
using Application.Interfaces.Application.Interfaces.Services;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _imageRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoriteService> _logger;

        public FavoriteService(
            IFavoriteRepository favoriteRepo,
            IProductRepository productRepo,
            IProductImageRepository imageRepo,
            ICategoryRepository categoryRepo,
            IMapper mapper,
            ILogger<FavoriteService> logger)
        {
            _favoriteRepo = favoriteRepo;
            _productRepo = productRepo;
            _imageRepo = imageRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetUserFavoritesAsync(int userId)
        {
            _logger.LogInformation("در حال دریافت لیست علاقه‌مندی‌های کاربر با شناسه {UserId}", userId);

            var favorites = await _favoriteRepo.GetByUserIdAsync(userId);
            if (favorites == null || !favorites.Any())
            {
                _logger.LogWarning("هیچ محصولی در لیست علاقه‌مندی کاربر با شناسه {UserId} وجود ندارد", userId);
                return new List<ProductDto>();
            }

            var productIds = favorites.Select(f => f.ProductId).ToList();
            var products = await _productRepo.GetByIdsAsync(productIds);
            if (products == null || !products.Any())
            {
                _logger.LogWarning("محصولات مربوط به علاقه‌مندی‌های کاربر {UserId} یافت نشدند", userId);
                throw new AppException(
                    "محصولات مورد علاقه یافت نشدند.",
                    StatusCodes.Status404NotFound,
                    ErrorCode.ProductsNotFound);
            }

            var images = await _imageRepo.GetByProductIdsAsync(productIds) ?? new List<ProductImage>();
            var categories = await _categoryRepo.GetAllAsync() ?? new List<Category>();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            foreach (var p in productDtos)
            {
                p.ImageUrls = images
                    .Where(i => i.ProductId == p.Id)
                    .Select(i => i.Url)
                    .ToList();

                p.ImageUrl = p.ImageUrls.FirstOrDefault() ?? string.Empty;

                if (categoryDict.TryGetValue(p.CategoryId, out var categoryName))
                    p.CategoryName = categoryName;
            }

            _logger.LogInformation("لیست علاقه‌مندی‌های کاربر {UserId} با موفقیت بازیابی شد", userId);
            return productDtos;
        }

        public async Task<bool> IsFavoriteAsync(int userId, int productId)
        {
            _logger.LogInformation("بررسی علاقه‌مندی محصول {ProductId} برای کاربر {UserId}", productId, userId);

            var exists = await _favoriteRepo.ExistsAsync(userId, productId);
            return exists;
        }

        public async Task AddToFavoriteAsync(int userId, int productId)
        {
            _logger.LogInformation("در حال افزودن محصول {ProductId} به علاقه‌مندی‌های کاربر {UserId}", productId, userId);

            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("محصول {ProductId} برای افزودن به علاقه‌مندی‌ها یافت نشد", productId);
                throw new AppException(
                    "محصول مورد نظر یافت نشد.",
                    StatusCodes.Status404NotFound,
                    ErrorCode.ProductNotFound);
            }

            var alreadyExists = await _favoriteRepo.ExistsAsync(userId, productId);
            if (alreadyExists)
            {
                _logger.LogWarning("محصول {ProductId} قبلاً در علاقه‌مندی‌های کاربر {UserId} وجود داشته است", productId, userId);
                throw new AppException(
                    "این محصول قبلاً به علاقه‌مندی‌ها اضافه شده است.",
                    StatusCodes.Status409Conflict,
                    ErrorCode.AlreadyExistsInFavorites);
            }

            await _favoriteRepo.AddAsync(new Favorite
            {
                UserId = userId,
                ProductId = productId
            });

            _logger.LogInformation("محصول {ProductId} با موفقیت به علاقه‌مندی‌های کاربر {UserId} اضافه شد", productId, userId);
        }

        public async Task RemoveFromFavoriteAsync(int userId, int productId)
        {
            _logger.LogInformation("در حال حذف محصول {ProductId} از علاقه‌مندی‌های کاربر {UserId}", productId, userId);

            var exists = await _favoriteRepo.ExistsAsync(userId, productId);
            if (!exists)
            {
                _logger.LogWarning("محصول {ProductId} در علاقه‌مندی‌های کاربر {UserId} یافت نشد", productId, userId);
                throw new AppException(
                    "محصول در لیست علاقه‌مندی‌ها وجود ندارد.",
                    StatusCodes.Status404NotFound,
                    ErrorCode.FavoriteItemNotFound);
            }

            await _favoriteRepo.RemoveByUserAndProductAsync(userId, productId);
            _logger.LogInformation("محصول {ProductId} با موفقیت از علاقه‌مندی‌های کاربر {UserId} حذف شد", productId, userId);
        }
    }
}
