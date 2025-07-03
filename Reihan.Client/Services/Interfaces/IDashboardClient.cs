using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IDashboardClient
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<List<RecentOrderDto>> GetRecentOrdersAsync();
        Task<List<SalesChartDto>> GetSalesChartAsync(DateTime from, DateTime to);
    }
}
