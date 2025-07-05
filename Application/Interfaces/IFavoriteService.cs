using Application.DTOs;

namespace Application.Interfaces
{
    namespace Application.Interfaces.Services
    {
        public interface IFavoriteService
        {
            Task<List<ProductDto>> GetFavoriteProductsAsync(int userId);
            Task<bool> IsFavoriteAsync(int userId, int productId);
            Task AddToFavoriteAsync(int userId, int productId);
            Task RemoveFromFavoriteAsync(int userId, int productId);
        }
    }
}
