using Reihan.Client.Models.Cart;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class CartClient : ICartClient
    {
        private readonly HttpClient _http;

        public CartClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<CartDto?> GetCartAsync()
        {
            return await _http.GetFromJsonAsync<CartDto>("api/cart");
        }

        public async Task AddItemAsync(int productId, int quantity = 1)
        {
            await _http.PostAsJsonAsync("api/cart/items", new { ProductId = productId, Quantity = quantity });
        }

        public async Task RemoveItemAsync(int productId)
        {
            await _http.DeleteAsync($"api/cart/items/{productId}");
        }

        public async Task ClearAsync()
        {
            await _http.DeleteAsync("api/cart");
        }
    }

}
