using MudBlazor;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class UserAddressClient : IUserAddressClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public UserAddressClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<List<UserAddressDto>> GetAllAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/user-addresses");
                var result = await response.HandleResponseAsync<List<UserAddressDto>>(_snackbar);
                return result ?? new List<UserAddressDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<UserAddressDto>();
            }
        }

        public async Task CreateAsync(UserAddressDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/user-addresses", dto);
                await response.HandleResponseAsync(_snackbar, "آدرس اضافه شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/user-addresses/{id}");
                await response.HandleResponseAsync(_snackbar, "آدرس حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task SetDefault(int addressId)
        {
            try
            {
                var response = await _http.PutAsync($"api/user-addresses/{addressId}/set-default", null);
                await response.HandleResponseAsync(_snackbar, "آدرس پیشفرض انتخاب شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task<UserAddressDto?> GetDefault()
        {
            try
            {
                var response = await _http.GetAsync("api/user-addresses/get-default");
                var result = await response.HandleResponseAsync<UserAddressDto>(_snackbar);
                return result ?? new UserAddressDto();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new UserAddressDto();
            }
        }
    }
}
