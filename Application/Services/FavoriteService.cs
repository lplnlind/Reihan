using Application.DTOs;
using Application.Interfaces.Application.Interfaces.Services;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _imageRepo;
        private readonly ICategoryRepository _categoryRepo;

        public FavoriteService(
            IFavoriteRepository favoriteRepo,
            IProductRepository productRepo,
            IProductImageRepository imageRepo,
            ICategoryRepository categoryRepo)
        {
            _favoriteRepo = favoriteRepo;
            _productRepo = productRepo;
            _imageRepo = imageRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<ProductDto>> GetFavoriteProductsAsync(int userId)
        {
            var favorites = await _favoriteRepo.GetByUserIdAsync(userId);
            var productIds = favorites.Select(f => f.ProductId).ToHashSet();

            var products = await _productRepo.GetAllAsync();
            var images = await _imageRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            return products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = categoryDict.GetValueOrDefault(p.CategoryId),
                    StockQuantity = p.StockQuantity,
                    ImageUrls = images.Where(i => i.ProductId == p.Id).Select(i => i.Url).ToList()
                }).ToList();
        }

        public async Task<bool> IsFavoriteAsync(int userId, int productId)
            => await _favoriteRepo.ExistsAsync(userId, productId);

        public async Task AddToFavoriteAsync(int userId, int productId)
        {
            if (!await _favoriteRepo.ExistsAsync(userId, productId))
                await _favoriteRepo.AddAsync(new Favorite { UserId = userId, ProductId = productId });
        }

        public async Task RemoveFromFavoriteAsync(int userId, int productId)
            => await _favoriteRepo.RemoveByUserAndProductAsync(userId, productId);
    }
}
