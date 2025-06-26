using Reihan.Client.Models.Order;
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

        public async Task<int> CreateOrderAsync(CreateOrderRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/orders", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            return await _http.GetFromJsonAsync<List<OrderDto>>("api/orders");
        }
    }

}
