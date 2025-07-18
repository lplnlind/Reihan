using MudBlazor;
using Reihan.Client.Enums;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public OrderClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/orders");
                var result = await response.HandleResponseAsync<List<OrderDto>>(_snackbar);
                return result ?? new List<OrderDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<OrderDto>();
            }
        }

        public async Task<List<OrderDetailsDto>> GetOrderDetailsAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"api/orders/{id}/details");
                var result = await response.HandleResponseAsync<List<OrderDetailsDto>>(_snackbar);
                return result ?? new List<OrderDetailsDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<OrderDetailsDto>();
            }
        }

        public async Task UpdateOrderStatusAsync(int id, OrderStatus newStatus)
        {
            try
            {
                var req = new { NewStatus = newStatus };
                var response = await _http.PutAsJsonAsync($"api/orders/{id}/status", req);
                await response.HandleResponseAsync(_snackbar, "وضعیت سفارش به روز شد");
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
            }
        }

        public async Task<List<OrderDetailsDto>> GetOrdersByUserAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/orders/user");
                var result = await response.HandleResponseAsync<List<OrderDetailsDto>>(_snackbar);
                return result ?? new List<OrderDetailsDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<OrderDetailsDto>();
            }
        }

        public async Task<int> CreateAsync(CreateOrderRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/orders", request);
                var result = await response.HandleResponseAsync<int>(_snackbar);
                return result;
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return 0;
            }
        }
    }
}
