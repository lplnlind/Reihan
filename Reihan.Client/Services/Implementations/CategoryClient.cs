using MudBlazor;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class CategoryClient : ICategoryClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public CategoryClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories");
                return result ?? new();
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در بارگذاری دسته‌بندی‌ها", Severity.Error);
                return new();
            }
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<CategoryDto>($"api/categories/{id}");
            }
            catch (Exception)
            {
                _snackbar.Add("خطا در دریافت اطلاعات دسته‌بندی", Severity.Error);
                return null;
            }
        }

        public async Task CreateAsync(CategoryDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/categories", dto);
                await response.HandleResponseAsync(_snackbar, "دسته‌بندی با موفقیت اضافه شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task UpdateAsync(CategoryDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/categories/{dto.Id}", dto);
                await response.HandleResponseAsync(_snackbar, "دسته‌بندی بروزرسانی شد");
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
                var response = await _http.DeleteAsync($"api/categories/{id}");
                await response.HandleResponseAsync(_snackbar, "دسته‌بندی حذف شد", Severity.Warning);
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }
    }
}
