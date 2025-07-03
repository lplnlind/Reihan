using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<List<RecentOrderDto>> GetRecentOrdersAsync(int count = 5);
        Task<List<SalesChartDto>> GetSalesChartAsync(DateTime from, DateTime to);
    }
}
