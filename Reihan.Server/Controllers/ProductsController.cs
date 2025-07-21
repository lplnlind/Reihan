using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reihan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private readonly IUserContextService _userContext;

        public ProductsController(IProductService productService, 
            IUserContextService userContext,
            IImageService imageService)
        {
            _productService = productService;
            _userContext = userContext;
            _imageService = imageService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            bool includeInactive = _userContext.IsAdmin();
            var products = await _productService.GetAllProductsAsync(includeInactive);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            await _productService.AddProductAsync(dto);
            return Ok("محصول با موفقیت اضافه شد.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            if (id != dto.Id) return BadRequest("شناسه با مدل تطابق ندارد.");

            await _productService.UpdateProductAsync(dto);
            return Ok("محصول ویرایش شد.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("محصول حذف شد.");
        }

        [HttpGet("latest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestProducts() =>
            Ok(await _productService.GetLatestProductsAsync());

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> Filter([FromQuery] int? categoryId)
        {
            var result = await _productService.GetFilteredProductsAsync(categoryId);
            return Ok(result);
        }

        [HttpGet("{productId}/is")]
        [AllowAnonymous]
        public async Task<IActionResult> IsInCart(int productId)
        {
            var userId = _userContext.GetUserId();
            var result = await _productService.IsInCartAsync(userId, productId);
            return Ok(result);
        }

        [HttpPut("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int id)
        {
            await _productService.SetActiveStatusAsync(id, true);
            return NoContent();
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _productService.SetActiveStatusAsync(id, false);
            return NoContent();
        }
    }
}
