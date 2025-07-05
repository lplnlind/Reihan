using Application.Interfaces;
using Application.Interfaces.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reihan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IUserContextService _userContext;

        public FavoriteController(IFavoriteService favoriteService, IUserContextService userContext)
        {
            _favoriteService = favoriteService;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _userContext.GetUserId();
            var result = await _favoriteService.GetFavoriteProductsAsync(userId);
            return Ok(result);
        }

        [HttpGet("{productId}/is")]
        public async Task<IActionResult> IsFavorite(int productId)
        {
            var userId = _userContext.GetUserId();
            var result = await _favoriteService.IsFavoriteAsync(userId, productId);
            return Ok(result);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = _userContext.GetUserId();
            await _favoriteService.AddToFavoriteAsync(userId, productId);
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = _userContext.GetUserId();
            await _favoriteService.RemoveFromFavoriteAsync(userId, productId);
            return Ok();
        }
    }
}