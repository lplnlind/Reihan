using MudBlazor;
using Reihan.Shared.DTOs;
using System.Net.Http.Json;

namespace Reihan.Client.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T?> HandleResponseAsync<T>(this HttpResponseMessage response, ISnackbar snackbar)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }

            var error = await response.Content.ReadFromJsonAsync<ApiErrorDto>();
            snackbar.Add(error?.Message ?? "خطای ناشناخته‌ای رخ داده است.", Severity.Error);
            return default;
        }

        public static async Task HandleResponseAsync(this HttpResponseMessage response, ISnackbar snackbar, string successMessage = "عملیات با موفقیت انجام شد", Severity severity = Severity.Success)
        {
            if (response.IsSuccessStatusCode)
            {
                snackbar.Add(successMessage, severity);
                return;
            }

            var error = await response.Content.ReadFromJsonAsync<ApiErrorDto>();
            snackbar.Add(error?.Message ?? "خطای ناشناخته‌ای رخ داده است.", Severity.Error);
        }
    }

}
