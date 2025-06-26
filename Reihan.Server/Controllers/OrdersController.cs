using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reihan.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserContextService _userContext;

        public OrdersController(IOrderService orderService, IUserContextService userContext)
        {
            _orderService = orderService;
            _userContext = userContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var userId = _userContext.GetUserId();
            var result = await _orderService.CreateOrderAsync(userId, request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = _userContext.GetUserId();
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }
    }
}
