using MudBlazor;
using Reihan.Client.Extensions;
using Reihan.Shared.DTOs;
using Reihan.Client.Services;
using System.Net;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class FavoriteClient : IFavoriteClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public FavoriteClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<List<ProductDto>> GetUserFavoritesAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/favorite");
                var result = await response.HandleResponseAsync<List<ProductDto>>(_snackbar);
                return result ?? new List<ProductDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<ProductDto>();
            }
        }

        public async Task<bool> IsFavoriteAsync(int productId)
        {
            try
            {
                var response = await _http.GetAsync($"api/favorite/{productId}/is");
                var result = await response.HandleResponseAsync<bool>(_snackbar);
                return result;
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return false;
            }
        }

        public async Task AddToFavoriteAsync(int productId)
        {
            try
            {
                var response = await _http.PostAsync($"api/favorite/{productId}", null);
                await response.HandleResponseAsync(_snackbar, "محصول به علاقه‌مندی‌ها اضافه شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task RemoveFromFavoriteAsync(int productId)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/favorite/{productId}");
                await response.HandleResponseAsync(_snackbar, "محصول از علاقه‌مندی‌ها حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }
    }
}
