using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _http;

        public OrderClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<OrderDto>>("api/orders");
        }

        public async Task UpdateOrderStatusAsync(int id, string newStatus)
        {
            var req = new { NewStatus = newStatus };
            await _http.PutAsJsonAsync($"api/orders/{id}/status", req);
        }

        public async Task<List<OrderDto>> GetOrdersByUserAsync()
        {
            return await _http.GetFromJsonAsync<List<OrderDto>>("api/orders/user");
        }

        public async Task<int> CreateAsync(CreateOrderRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/orders", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
