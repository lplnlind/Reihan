using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IFavoriteClient
    {
        Task<List<ProductDto>> GetFavoritesAsync();
        Task<bool> IsFavoriteAsync(int productId);
        Task AddToFavoriteAsync(int productId);
        Task RemoveFromFavoriteAsync(int productId);
    }
}
