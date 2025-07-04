using Reihan.Client.Models;
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

        public async Task AddItemAsync(AddToCartRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/cart/add", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveItemAsync(int productId)
        {
            var response = await _http.DeleteAsync($"api/cart/remove/{productId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearAsync()
        {
            await _http.DeleteAsync("api/cart");
        }
    }

}
