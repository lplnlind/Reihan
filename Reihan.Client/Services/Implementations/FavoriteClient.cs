using Reihan.Client.Models;
using Reihan.Client.Services;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class FavoriteClient : IFavoriteClient
    {
        private readonly HttpClient _http;
        public FavoriteClient(HttpClient http) => _http = http;

        public Task<List<ProductDto>> GetFavoritesAsync() => _http.GetFromJsonAsync<List<ProductDto>>("api/favorite")!;

        public Task<bool> IsFavoriteAsync(int productId) => _http.GetFromJsonAsync<bool>($"api/favorite/{productId}/is")!;

        public Task AddToFavoriteAsync(int productId) => _http.PostAsync($"api/favorite/{productId}", null);

        public Task RemoveFromFavoriteAsync(int productId) => _http.DeleteAsync($"api/favorite/{productId}");
    }

}
