using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface ICartClient
    {
        Task<CartDto?> GetCartAsync();
        Task AddItemAsync(AddToCartRequest request);
        Task RemoveItemAsync(int productId);
        Task ChangeQuantity(int productId, int quantity);
        Task ClearAsync();
    }
}
