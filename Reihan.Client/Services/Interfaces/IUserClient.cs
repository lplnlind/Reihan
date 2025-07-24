using Reihan.Shared.Enums;
using Reihan.Shared.DTOs;

namespace Reihan.Client.Services
{
    public interface IUserClient
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserRoleAsync(int userId, SharedUserRole newRole);
        Task ToggleUserStatusAsync(int userId, bool isActive);
        Task<UserDto> GetByIdAsync(int userId);
    }
}
