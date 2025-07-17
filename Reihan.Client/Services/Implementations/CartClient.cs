using Reihan.Client.Extensions;
using Reihan.Client.Models;
using MudBlazor;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class CartClient : ICartClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public CartClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<CartDto?> GetCartAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/cart");
                return await response.HandleResponseAsync<CartDto>(_snackbar) ?? new CartDto();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new CartDto();
            }
        }

        public async Task AddItemAsync(AddToCartRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/cart/add", request);
                await response.HandleResponseAsync(_snackbar, "محصول با موفقیت به سبد اضافه شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task RemoveItemAsync(int productId)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/cart/remove/{productId}");
                await response.HandleResponseAsync(_snackbar, "محصول با موفقیت حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task ChangeQuantity(int productId, int quantity)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/cart/quantity/{productId}", quantity);
                await response.HandleResponseAsync(_snackbar, "تعداد محصول به‌روزرسانی شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                var response = await _http.DeleteAsync("api/cart/clear");
                await response.HandleResponseAsync(_snackbar, "سبد خرید پاک‌سازی شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }
    }
}
