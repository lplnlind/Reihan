using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class UserAddressClient : IUserAddressClient
    {
        private readonly HttpClient _http;

        public UserAddressClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UserAddressDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<UserAddressDto>>("api/user-addresses") ?? new();
        }

        public async Task CreateAsync(UserAddressDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/user-addresses", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/user-addresses/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task SetDefault(int addressId)
        {
            var response = await _http.PutAsync($"api/user-addresses/{addressId}/set-default", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<UserAddressDto?> GetDefault()
        {
            return await _http.GetFromJsonAsync<UserAddressDto>("api/user-addresses/get-default");
        }
    }
}
