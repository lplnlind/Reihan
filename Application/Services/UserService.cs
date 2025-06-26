using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Persistence.Repositories;
using System.Data;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email.Value,
                Role = u.Role.ToString(),
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task UpdateUserRoleAsync(int userId, string newRole)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");

            if (!Enum.TryParse<UserRole>(newRole, out var role))
                throw new Exception("نقش نامعتبر است.");

            user.SetRole(role);
            await _userRepo.UpdateAsync(user);
        }

        public async Task ToggleUserStatusAsync(int userId, bool isActive)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");

            user.SetStatus(isActive);
            await _userRepo.UpdateAsync(user);
        }
    }
}
