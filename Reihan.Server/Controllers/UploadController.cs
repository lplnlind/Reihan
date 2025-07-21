using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reihan.Server.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IImageService imageService, ILogger<UploadController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpPost("product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadProductImage([FromForm] IFormFile image)
        {
            var url = await _imageService.UploadImageAsync(image, "products");
            return Ok(new { imageUrl = url });
        }

        [HttpPost("profile")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile image)
        {
            var url = await _imageService.UploadImageAsync(image, "profiles");
            return Ok(new { imageUrl = url });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage([FromQuery] string path)
        {
            await _imageService.DeleteImageAsync(path);
            return Ok();
        }

        [HttpDelete("by-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImageByUser([FromQuery] string path)
        {
            await _imageService.DeleteImageAsync(path);
            return Ok();
        }
    }
}
