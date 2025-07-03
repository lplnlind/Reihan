using Reihan.Client.Models;
using Reihan.Client.Services;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class DashboardClient : IDashboardClient
    {
        private readonly HttpClient _http;
        public DashboardClient(HttpClient http) => _http = http;

        public Task<DashboardStatsDto> GetStatsAsync() =>
            _http.GetFromJsonAsync<DashboardStatsDto>("api/admin/dashboard/stats")!;

        public Task<List<RecentOrderDto>> GetRecentOrdersAsync() =>
            _http.GetFromJsonAsync<List<RecentOrderDto>>("api/admin/dashboard/recent-orders")!;

        public async Task<List<SalesChartDto>> GetSalesChartAsync(DateTime from, DateTime to)
        {
            return await _http.GetFromJsonAsync<List<SalesChartDto>>(
                $"api/admin/dashboard/sales-chart?from={from:O}&to={to:O}")!;
        }
    }
}
