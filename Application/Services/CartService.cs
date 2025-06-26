using Application.Interfaces.Repositories;
using Application.Interfaces;
using Domain.Entities;
using Application.DTOs;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId) ?? new Cart(userId);
            return new CartDto
            {
                UserId = userId,
                TotalPrice = cart.GetTotalPrice(),
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ProductName = i.ProductName
                }).ToList()
            };
        }

        public async Task AddItemAsync(int userId, int productId, int quantity, decimal price, string productName)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId) ?? new Cart(userId);
            cart.AddItem(productId, quantity, price, productName);
            if (cart.Id == 0)
                await _cartRepository.AddAsync(cart);
            else
                await _cartRepository.UpdateAsync(cart);
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null) return;
            cart.RemoveItem(productId);
            await _cartRepository.UpdateAsync(cart);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart is null) return;
            cart.Clear();
            await _cartRepository.UpdateAsync(cart);
        }
    }

}
