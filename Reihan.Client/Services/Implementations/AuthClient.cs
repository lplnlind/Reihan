using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Reihan.Client.Authentication;
using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthStateProvider _stateProvider;
        private const string TokenKey = "authToken";

        public AuthClient(ILocalStorageService localStorage, 
            HttpClient http, AuthenticationStateProvider authProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _stateProvider = (CustomAuthStateProvider)authProvider;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result is null || string.IsNullOrWhiteSpace(result.Token))
                return false;

            await _localStorage.SetItemAsync("authToken", result.Token);
            _stateProvider.NotifyUserAuthentication(result.Token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result is null || string.IsNullOrWhiteSpace(result.Token))
                return false;

            await _localStorage.SetItemAsync("authToken", result.Token);
            _stateProvider.NotifyUserAuthentication(result.Token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/reset-password", request);
            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _http.DefaultRequestHeaders.Authorization = null;

            if (_stateProvider is CustomAuthStateProvider authProvider)
                authProvider.NotifyUserLogout();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }

        public async Task<UserProfileDto> GetProfileAsync()
        {
            return await _http.GetFromJsonAsync<UserProfileDto>("api/auth/profile");
        }

        public async Task UpdateProfileAsync(UpdateProfileRequest request)
        {
            var response = await _http.PutAsJsonAsync("api/auth/profile", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = await _http.PutAsJsonAsync("api/auth/change-password", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
