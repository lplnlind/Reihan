using Reihan.Shared.DTOs;

namespace Application.Interfaces
{
    namespace Application.Interfaces.Services
    {
        public interface IFavoriteService
        {
            Task<List<ProductDto>> GetUserFavoritesAsync(int userId);
            Task<bool> IsFavoriteAsync(int userId, int productId);
            Task AddToFavoriteAsync(int userId, int productId);
            Task RemoveFromFavoriteAsync(int userId, int productId);
        }
    }
}
