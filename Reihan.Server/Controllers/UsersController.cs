using Reihan.Shared.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;

namespace Reihan.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, UpdateUserRoleRequest request)
        {
            await _userService.UpdateUserRoleAsync(id, (UserRole)(int)request.NewRole);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ToggleStatus(int id, ToggleUserStatusRequest request)
        {
            await _userService.ToggleUserStatusAsync(id, request.IsActive);
            return NoContent();
        }
    }
}
