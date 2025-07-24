using Reihan.Shared.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Reihan.Shared.Enums;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepo, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user is null)
            {
                _logger.LogWarning("کاربر با شناسه {Id} یافت نشد", userId);
                throw new AppException("کاربر یافت نشد",
                    StatusCodes.Status404NotFound,
                    ErrorCode.ProductNotFound);
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new AppException("کاربر یافت نشد.",
                    StatusCodes.Status404NotFound,
                    ErrorCode.UserNotFound);

            if (!Enum.TryParse<UserRole>(newRole.ToString(), out var role))
                throw new AppException("نقش نامعتبر است.", 
                    StatusCodes.Status400BadRequest,
                    ErrorCode.InvalidUserRole);

            user.SetRole(role);
            await _userRepo.UpdateAsync(user);
        }

        public async Task ToggleUserStatusAsync(int userId, bool isActive)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new AppException("کاربر یافت نشد.",
                    StatusCodes.Status404NotFound,
                    ErrorCode.UserNotFound);

            user.SetStatus(isActive);
            await _userRepo.UpdateAsync(user);
        }
    }
}
