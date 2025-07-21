using MudBlazor;
using Reihan.Client.Extensions;
using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public class UploadClient : IUploadClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public UploadClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<ImageUploadResult> UploadProductImageAsync(MultipartFormDataContent content)
        {
            try
            {
                var response = await _http.PostAsync("api/upload/product", content);
                var result = await response.HandleResponseAsync<ImageUploadResult>(_snackbar);
                return result ?? new ImageUploadResult();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new ImageUploadResult();
            }
        }

        public async Task<ImageUploadResult> UploadProfileImageAsync(MultipartFormDataContent content)
        {
            try
            {
                var response = await _http.PostAsync("api/upload/profile", content);
                var result = await response.HandleResponseAsync<ImageUploadResult>(_snackbar);
                return result ?? new ImageUploadResult();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new ImageUploadResult();
            }
        }

        public async Task DeleteImageAsync(string path)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/upload?path={path}");
                await response.HandleResponseAsync(_snackbar, "تصویر حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task DeleteImageByUserAsync(string path)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/upload/by-user?path={path}");
                await response.HandleResponseAsync(_snackbar, "تصویر حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }
    }
}
