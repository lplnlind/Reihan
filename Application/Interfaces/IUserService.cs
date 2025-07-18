using Application.DTOs;
using Domain.Enums;
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
        Task UpdateUserRoleAsync(int userId, UserRole newRole);
        Task ToggleUserStatusAsync(int userId, bool isActive);
    }
}
