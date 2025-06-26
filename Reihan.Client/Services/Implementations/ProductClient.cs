using Reihan.Client.Models;
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
    }
}
