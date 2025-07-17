using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Reihan.Client.Authentication;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Reihan.Client.Services
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthStateProvider _stateProvider;
        private readonly ISnackbar _snackbar;
        private const string TokenKey = "authToken";

        public AuthClient(ILocalStorageService localStorage,
            HttpClient http,
            AuthenticationStateProvider authProvider,
            ISnackbar snackbar)
        {
            _localStorage = localStorage;
            _http = http;
            _stateProvider = (CustomAuthStateProvider)authProvider;
            _snackbar = snackbar;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", request);
                var result = await response.HandleResponseAsync<AuthResponse>(_snackbar);

                if (result is null || string.IsNullOrWhiteSpace(result.Token))
                    return false;

                await _localStorage.SetItemAsync(TokenKey, result.Token);
                _stateProvider.NotifyUserAuthentication(result.Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

                return true;
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در ورود به حساب کاربری", Severity.Error);
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/register", request);
                var result = await response.HandleResponseAsync<AuthResponse>(_snackbar);

                if (result is null || string.IsNullOrWhiteSpace(result.Token))
                    return false;

                await _localStorage.SetItemAsync(TokenKey, result.Token);
                _stateProvider.NotifyUserAuthentication(result.Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

                return true;
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در ثبت‌نام", Severity.Error);
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/reset-password", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در بازنشانی گذرواژه", Severity.Error);
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync(TokenKey);
                _http.DefaultRequestHeaders.Authorization = null;
                _stateProvider.NotifyUserLogout();
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در خروج از حساب کاربری", Severity.Warning);
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<string>(TokenKey);
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در دریافت توکن", Severity.Warning);
                return null;
            }
        }

        public async Task<UserProfileDto?> GetProfileAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/auth/profile");
                return await response.HandleResponseAsync<UserProfileDto>(_snackbar);
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در دریافت پروفایل", Severity.Error);
                return null;
            }
        }

        public async Task UpdateProfileAsync(UpdateProfileRequest request)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("api/auth/profile", request);
                await response.HandleResponseAsync(_snackbar);
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در به‌روزرسانی اطلاعات کاربر", Severity.Error);
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("api/auth/change-password", request);
                await response.HandleResponseAsync(_snackbar);
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در تغییر گذرواژه", Severity.Error);
            }
        }

        public async Task<JwtUserDto?> GetCurrentUserAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(TokenKey);
                if (string.IsNullOrWhiteSpace(token)) return null;

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                return new JwtUserDto
                {
                    Id = int.Parse(jwt.Claims.First(c => c.Type == "sub").Value),
                    FullName = jwt.Claims.FirstOrDefault(c => c.Type == "fullname")?.Value ?? "",
                    Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? ""
                };
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در دریافت اطلاعات کاربر جاری", Severity.Error);
                return null;
            }
        }
    }
}
