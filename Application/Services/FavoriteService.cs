using Application.DTOs;
using Application.Interfaces.Application.Interfaces.Services;
using Application.Interfaces.Repositories;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public FavoriteService(
            IFavoriteRepository favoriteRepo,
            IProductRepository productRepo,
            IProductImageRepository imageRepo,
            ICategoryRepository categoryRepo,
            IMapper mapper)
        {
            _favoriteRepo = favoriteRepo;
            _productRepo = productRepo;
            _imageRepo = imageRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _favoriteRepo.GetByUserIdAsync(userId);
            var productIds = favorites.Select(f => f.ProductId).ToList();

            var products = await _productRepo.GetByIdsAsync(productIds);
            var images = await _imageRepo.GetByProductIdsAsync(productIds);

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            foreach (var p in productDtos)
            {
                p.ImageUrls = images.Where(i => i.ProductId == p.Id).Select(i => i.Url).ToList();
                p.ImageUrl = images.FirstOrDefault(f => f.ProductId == p.Id)?.Url ?? string.Empty;
            }

            return productDtos;
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
