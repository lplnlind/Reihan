using Reihan.Shared.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Domain.Enums;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IUserContextService userContextService,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            _logger.LogInformation("در حال ثبت‌نام کاربر با نام کاربری: {UserName}", request.UserName);

            var existingUser = await _userRepository.GetByUsernameAsync(request.UserName);
            if (existingUser is not null)
            {
                _logger.LogWarning("نام کاربری تکراری: {UserName}", request.UserName);
                throw new AppException("نام کاربری قبلاً ثبت شده است.", 409, ErrorCode.UsernameAlreadyExists);
            }

            var hashedPassword = _passwordHasher.HashPassword(new User(), request.Password);
            var user = _mapper.Map<User>(request);
            user.PasswordHash = hashedPassword;
            user.Role = UserRole.Customer;

            await _userRepository.AddAsync(user);

            _logger.LogInformation("ثبت‌نام موفق برای {UserName}", user.UserName);

            var token = _jwtTokenGenerator.GenerateToken(_mapper.Map<JwtUserDto>(user));

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("درخواست ورود با ایمیل یا نام کاربری: {UserNameOrEmail}", request.UserNameOrEmail);

            User? user;
            if (request.UserNameOrEmail.Contains("@"))
                user = await _userRepository.GetByEmailAsync(request.UserNameOrEmail);
            else
                user = await _userRepository.GetByUsernameAsync(request.UserNameOrEmail);

            if (user == null)
            {
                _logger.LogWarning("کاربر یافت نشد: {Input}", request.UserNameOrEmail);
                throw new AppException("نام کاربری یا رمز عبور اشتباه است.", 400, ErrorCode.InvalidLoginCredentials);
            }

            if (!await ValidatePasswordAsync(request.Password, user.UserName))
            {
                _logger.LogWarning("رمز نادرست برای کاربر: {UserName}", user.UserName);
                throw new AppException("نام کاربری یا رمز عبور اشتباه است.", 400, ErrorCode.InvalidLoginCredentials);
            }

            var jwtUser = _mapper.Map<JwtUserDto>(user);
            var token = _jwtTokenGenerator.GenerateToken(jwtUser);

            _logger.LogInformation("ورود موفق برای {UserName}", user.UserName);

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }

        public async Task<UserProfileDto?> GetProfileAsync(ClaimsPrincipal user)
        {
            var username = user.Identity?.Name;

            _logger.LogInformation("درخواست پروفایل برای: {Username}", username);

            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("نام کاربری موجود نیست در Claims");
                throw new AppException("داده ارسالی نامعتبر است", 400, ErrorCode.UserNotFound);
            }

            var dbUser = await _userRepository.GetByUsernameAsync(username);
            if (dbUser == null)
            {
                _logger.LogWarning("کاربر در دیتابیس یافت نشد: {Username}", username);
                throw new AppException("کاربر پیدا نشد", 404, ErrorCode.UserNotFound);
            }

            _logger.LogInformation("پروفایل با موفقیت دریافت شد: {Username}", username);
            return _mapper.Map<UserProfileDto>(dbUser);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string userName)
        {
            _logger.LogInformation("درخواست بررسی رمز برای: {Username}", userName);

            var user = await _userRepository.GetByUsernameAsync(userName);
            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("رمز اشتباه برای کاربر: {Username}", userName);
                return false;
            }

            return true;
        }

        public async Task UpdateProfileAsync(UpdateProfileRequest request)
        {
            _logger.LogInformation("درخواست بروزرسانی پروفایل برای: {Username}", request.UserName);

            var user = await _userRepository.GetByIdAsync(_userContextService.GetUserId());
            if (user is null)
            {
                _logger.LogWarning("کاربر برای بروزرسانی یافت نشد");
                throw new AppException("کاربر پیدا نشد", 404, ErrorCode.UserNotFound);
            }

            user.UpdateProfile(_mapper.Map<User>(request));
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("پروفایل با موفقیت بروزرسانی شد: {Username}", request.UserName);
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(_userContextService.GetUserId());
            if (user is null)
            {
                _logger.LogWarning("کاربر برای تغییر رمز یافت نشد");
                throw new AppException("کاربر پیدا نشد", 404, ErrorCode.UserNotFound);
            }

            _logger.LogInformation("درخواست تغییر رمز برای: {Username}", user.UserName);

            if (!await ValidatePasswordAsync(request.CurrentPassword, user.UserName))
            {
                _logger.LogWarning("رمز فعلی نادرست است: {Username}", user.UserName);
                throw new AppException("رمز فعلی نادرست است", 409, ErrorCode.InvalidCurrentPassword);
            }

            user.ChangePassword(_passwordHasher.HashPassword(user, request.NewPassword));
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("رمز عبور با موفقیت تغییر یافت: {Username}", user.UserName);
        }
    }
}
