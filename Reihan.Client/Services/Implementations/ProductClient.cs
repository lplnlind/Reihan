using MudBlazor;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.Net;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public ProductClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/products");
                var result = await response.HandleResponseAsync<List<ProductDto>>(_snackbar);
                return result ?? new List<ProductDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<ProductDto>();
            }
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"api/products/{id}");
                var result = await response.HandleResponseAsync<ProductDto>(_snackbar);
                return result ?? new ProductDto();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new ProductDto();
            }
        }

        public async Task CreateAsync(ProductDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/products", dto);
                await response.HandleResponseAsync(_snackbar, "محصول ثبت شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/products/{dto.Id}", dto);
                await response.HandleResponseAsync(_snackbar, "محصول ویرایش شد");
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
                var response = await _http.DeleteAsync($"api/products/{id}");
                await response.HandleResponseAsync(_snackbar, "محصول حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task<ImageUploadResult> Upload(MultipartFormDataContent content)
        {
            try
            {
                var response = await _http.PostAsync("api/products/upload-image", content);
                var result = await response.HandleResponseAsync<ImageUploadResult>(_snackbar);
                return result ?? new ImageUploadResult();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new ImageUploadResult();
            }
        }

        public async Task<List<ProductDto>> GetLatestAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/products/latest");
                var result = await response.HandleResponseAsync<List<ProductDto>>(_snackbar);
                return result ?? new List<ProductDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<ProductDto>();
            }
        }

        public async Task<List<ProductDto>> FilterAsync(int? categoryId = null)
        {
            try
            {
                var url = "api/products/filter";
                if (categoryId is not null)
                    url += $"?categoryId={categoryId}";

                var response = await _http.GetAsync(url);
                var result = await response.HandleResponseAsync<List<ProductDto>>(_snackbar);
                return result ?? new List<ProductDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<ProductDto>();
            }
        }

        public async Task<bool> IsInCartAsync(int productId)
        {
            try
            {
                var response = await _http.GetAsync($"api/products/{productId}/is");
                var result = await response.HandleResponseAsync<bool>(_snackbar);
                return result;
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return false;
            }
        }
    }
}
