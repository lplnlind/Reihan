using Reihan.Client.Models;
using Reihan.Client.Services;
using System.Net;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class FavoriteClient : IFavoriteClient
    {
        private readonly HttpClient _http;
        public FavoriteClient(HttpClient http) => _http = http;

        public async Task<List<ProductDto>> GetFavoritesAsync()
        {
            return await _http.GetFromJsonAsync<List<ProductDto>>("api/favorite") ?? new();
        }

        public async Task<bool> IsFavoriteAsync(int productId)
        {
            try
            {
                var response = await _http.GetAsync($"api/favorite/{productId}/is");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return false;

                response.EnsureSuccessStatusCode();
                var isFav = await response.Content.ReadFromJsonAsync<bool>();
                return isFav;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FavoriteClient] Error checking favorite: {ex.Message}");
                return false;
            }
        }

        public async Task AddToFavoriteAsync(int productId)
        {
            var response = await _http.PostAsync($"api/favorite/{productId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveFromFavoriteAsync(int productId)
        {
            var response = await _http.DeleteAsync($"api/favorite/{productId}");
            response.EnsureSuccessStatusCode();
        }
    }

}
