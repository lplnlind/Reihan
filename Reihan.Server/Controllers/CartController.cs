using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Reihan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = GetUserId(); // از توکن بخون
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartRequest request)
        {
            int userId = GetUserId();
            await _cartService.AddItemAsync(userId, request.ProductId, request.Quantity, request.UnitPrice, request.ProductName);
            return Ok();
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            int userId = GetUserId();
            await _cartService.RemoveItemAsync(userId, productId);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            int userId = GetUserId();
            await _cartService.ClearCartAsync(userId);
            return Ok();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
