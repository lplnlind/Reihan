using MudBlazor;
using Reihan.Client.Enums;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public UserClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/users");
                var result = await response.HandleResponseAsync<List<UserDto>>(_snackbar);
                return result ?? new List<UserDto>();
            }
            catch
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<UserDto>();
            }
        }

        public async Task UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            try
            {
                var req = new { NewRole = newRole };
                var response = await _http.PutAsJsonAsync($"api/users/{userId}/role", req);
                await response.HandleResponseAsync(_snackbar, "نقش کاربر به روزرسانی شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task ToggleUserStatusAsync(int userId, bool isActive)
        {
            try
            {
                var req = new { IsActive = isActive };
                var response = await _http.PutAsJsonAsync($"api/users/{userId}/status", req);
                await response.HandleResponseAsync(_snackbar, "وضعیت کاربر بروزرسانی شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }
    }
}
