using Reihan.Shared.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;

        public AdminService(IUserRepository userRepo,
            IProductRepository productRepo, 
            IOrderRepository orderRepo)
        {
            _userRepo = userRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var products = await _productRepo.GetAllAsync();
            var orders = await _orderRepo.GetAllAsync();

            return new DashboardStatsDto
            {
                TotalUsers = users.Count(),
                TotalProducts = products.Count(),
                TotalOrders = orders.Count(),
                TotalSales = orders.Sum(o => o.TotalAmount),
            };
        }

        public async Task<List<RecentOrderDto>> GetRecentOrdersAsync(int count = 5)
        {
            var orders = (await _orderRepo.GetAllAsync())
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToList();

            var users = await _userRepo.GetAllAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u.FullName);

            return orders.Select(o => new RecentOrderDto
            {
                OrderId = o.Id,
                UserFullName = userDict.GetValueOrDefault(o.UserId, ""),
                Amount = o.TotalAmount,
                Status = o.Status.ToString(),
                Date = o.OrderDate
            }).ToList();
        }

        public async Task<List<SalesChartDto>> GetSalesChartAsync(DateTime from, DateTime to)
        {
            var orders = await _orderRepo.GetAllAsync();

            return orders
                .Where(o => o.OrderDate.Date >= from.Date && o.OrderDate.Date <= to.Date)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new SalesChartDto
                {
                    Date = g.Key,
                    Amount = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

    }
}
