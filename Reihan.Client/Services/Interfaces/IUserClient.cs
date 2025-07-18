using Reihan.Client.Enums;
using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IUserClient
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserRoleAsync(int userId, UserRole newRole);
        Task ToggleUserStatusAsync(int userId, bool isActive);
    }
}
