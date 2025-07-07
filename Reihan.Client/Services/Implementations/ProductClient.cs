using Reihan.Client.Models;
using System.Net;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _http;

        public ProductClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<ProductDto>>("api/products") ?? new();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ProductDto>($"api/products/{id}");
        }

        public async Task CreateAsync(ProductDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/products", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/products/{dto.Id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ImageUploadResult> Upload(MultipartFormDataContent content)
        {
            var response = await _http.PostAsync("api/products/upload-image", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ImageUploadResult>();
                return result;
            }
            return null;
        }

        public async Task<List<ProductDto>> GetLatestAsync()
        {
            return await _http.GetFromJsonAsync<List<ProductDto>>("api/products/latest") ?? new();
        }

        public async Task<List<ProductDto>> FilterAsync(int? categoryId = null)
        {
            var url = "api/products/filter";
            if (categoryId is not null)
                url += $"?categoryId={categoryId}";

            return await _http.GetFromJsonAsync<List<ProductDto>>(url) ?? new();
        }

        public async Task<bool> IsInCartAsync(int productId)
        {
            try
            {
                var response = await _http.GetAsync($"api/products/{productId}/is");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return false;

                response.EnsureSuccessStatusCode();
                var isInCart = await response.Content.ReadFromJsonAsync<bool>();
                return isInCart;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FavoriteClient] Error checking favorite: {ex.Message}");
                return false;
            }
        }
    }
}
