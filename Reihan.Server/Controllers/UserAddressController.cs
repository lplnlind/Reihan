using Reihan.Shared.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reihan.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user-addresses")]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _service;
        private readonly IUserContextService _userContext;

        public UserAddressController(IUserAddressService service, IUserContextService userContext)
        {
            _service = service;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = _userContext.GetUserId();
            var addresses = await _service.GetUserAddressesAsync(userId);
            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserAddressDto dto)
        {
            int userId = _userContext.GetUserId();
            await _service.AddAddressAsync(userId, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}/set-default")]
        public async Task<IActionResult> SetDefault(int id)
        {
            int userId = _userContext.GetUserId();
            await _service.SetDefaultAsync(userId, id);
            return Ok();
        }

        [HttpGet("get-default")]
        public async Task<IActionResult> GetDefault()
        {
            int userId = _userContext.GetUserId();
            var address = await _service.GetDefaultAddressAsync(userId);
            return Ok(address);
        }
    }
}
