using Reihan.Shared.DTOs;
using Reihan.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
