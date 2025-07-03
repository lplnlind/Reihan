using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _http;

        public UserClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _http.GetFromJsonAsync<List<UserDto>>("api/users") ?? new();
        }

        public async Task UpdateUserRoleAsync(int userId, string newRole)
        {
            var req = new { NewRole = newRole };
            await _http.PutAsJsonAsync($"api/users/{userId}/role", req);
        }

        public async Task ToggleUserStatusAsync(int userId, bool isActive)
        {
            var req = new { IsActive = isActive };
            await _http.PutAsJsonAsync($"api/users/{userId}/status", req);
        }
    }
}
