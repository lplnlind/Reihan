using Reihan.Shared.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Reihan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserContextService _userContextService;

        public CartController(ICartService cartService, IUserContextService userContextService)
        {
            _cartService = cartService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = _userContextService.GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartRequest request)
        {
            int userId = _userContextService.GetUserId();
            await _cartService.AddItemAsync(userId, request);
            return Ok();
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            int userId = _userContextService.GetUserId();
            await _cartService.RemoveItemAsync(userId, productId);
            return Ok();
        }

        [HttpPut("quantity/{productId}")]
        public async Task<IActionResult> ChangeQuantity(int productId, [FromBody] int quantity)
        {
            int userId = _userContextService.GetUserId();
            await _cartService.ChangeQuantityAsync(userId, productId, quantity);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            int userId = _userContextService.GetUserId();
            await _cartService.ClearCartAsync(userId);
            return Ok();
        }
    }
}
