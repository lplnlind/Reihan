using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class CategoryClient : ICategoryClient
    {
        private readonly HttpClient _http;

        public CategoryClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories") ?? new();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<CategoryDto>($"api/categories/{id}");
        }

        public async Task CreateAsync(CategoryDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/categories", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(CategoryDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/categories/{dto.Id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
