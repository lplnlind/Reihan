using Reihan.Shared.DTOs;

namespace Reihan.Client.Services
{
    public interface IDashboardClient
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<List<RecentOrderDto>> GetRecentOrdersAsync();
        Task<List<SalesChartDto>> GetSalesChartAsync(DateTime from, DateTime to);
    }
}
