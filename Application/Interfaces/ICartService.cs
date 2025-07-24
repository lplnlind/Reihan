using Reihan.Shared.DTOs;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task AddItemAsync(int userId, AddToCartRequest request);
        Task RemoveItemAsync(int userId, int productId);
        Task ChangeQuantityAsync(int userId, int productId, int quantity);
        Task ClearCartAsync(int userId);
    }
}
