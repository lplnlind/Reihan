using MudBlazor;
using Reihan.Client.Extensions;
using Reihan.Client.Models;
using System.Net.Http.Json;

namespace Reihan.Client.Services
{
    public class DashboardClient : IDashboardClient
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;

        public DashboardClient(HttpClient http, ISnackbar snackbar)
        {
            _http = http;
            _snackbar = snackbar;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/admin/dashboard/stats");
                return await response.HandleResponseAsync<DashboardStatsDto>(_snackbar) ?? new DashboardStatsDto();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new DashboardStatsDto();
            }
        }

        public async Task<List<RecentOrderDto>> GetRecentOrdersAsync()
        {
            await _http.GetFromJsonAsync<List<RecentOrderDto>>("api/admin/dashboard/recent-orders")!;
            try
            {
                var response = await _http.GetAsync("api/admin/dashboard/recent-orders");
                return await response.HandleResponseAsync<List<RecentOrderDto>>(_snackbar) ?? new List<RecentOrderDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<RecentOrderDto>();
            }
        }

        public async Task<List<SalesChartDto>> GetSalesChartAsync(DateTime from, DateTime to)
        {
            try
            {
                var response = await _http.GetAsync($"api/admin/dashboard/sales-chart?from={from:O}&to={to:O}");
                return await response.HandleResponseAsync<List<SalesChartDto>>(_snackbar) ?? new List<SalesChartDto>();
            }
            catch (Exception)
            {
                _snackbar.Add("ارتباط با سرور برقرار نشد", Severity.Error);
                return new List<SalesChartDto>();
            }
        }
    }
}
