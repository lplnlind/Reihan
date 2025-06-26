using Application.DTOs;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task AddItemAsync(int userId, int productId, int quantity, decimal price, string productName);
        Task RemoveItemAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
