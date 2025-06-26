using Reihan.Client.Models.Cart;

namespace Reihan.Client.Services
{
    public interface ICartClient
    {
        Task<CartDto?> GetCartAsync();
        Task AddItemAsync(int productId, int quantity = 1);
        Task RemoveItemAsync(int productId);
        Task ClearAsync();
    }
}
