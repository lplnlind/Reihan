using Domain.Enums;
using Reihan.Shared.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task UpdateUserRoleAsync(int userId, UserRole newRole);
        Task ToggleUserStatusAsync(int userId, bool isActive);
    }
}
