using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reihan.Server.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public DashboardController(IAdminService adminService) => _adminService = adminService;

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats() =>
            Ok(await _adminService.GetDashboardStatsAsync());

        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders() =>
            Ok(await _adminService.GetRecentOrdersAsync());

        [HttpGet("sales-chart")]
        public async Task<IActionResult> GetSalesChart([FromQuery] DateTime from, [FromQuery] DateTime to) =>
            Ok(await _adminService.GetSalesChartAsync(from, to));
    }
}
