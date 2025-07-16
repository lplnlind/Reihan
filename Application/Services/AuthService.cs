using Application.DTOs.Auth;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IUserContextService userContextService,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.UserName);
            if (existingUser is not null)
                throw new Exception("نام کاربری قبلاً ثبت شده است.");

            var hashedPassword = _passwordHasher.HashPassword(new User(), request.Password);

            var user = _mapper.Map<User>(request);
            user.PasswordHash = hashedPassword;
            user.Role = UserRole.Customer;

            await _userRepository.AddAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(_mapper.Map<JwtUserDto>(user));

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role.ToDisplay()
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // بررسی اینکه کاربر چی وارد کرده: ایمیل یا نام کاربری
            User? user;
            if (request.UserNameOrEmail.Contains("@"))
            {
                user = await _userRepository.GetByEmailAsync(request.UserNameOrEmail);
            }
            else
            {
                user = await _userRepository.GetByUsernameAsync(request.UserNameOrEmail);
            }

            if (user == null)
                throw new Exception("نام کاربری یا رمز عبور اشتباه است.");

            if (!await ValidatePasswordAsync(request.Password, user.UserName))
                throw new Exception("نام کاربری یا رمز عبور اشتباه است.");

            var jwtUser = new JwtUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email.Value,
                Role = user.Role.ToString()
            };

            var token = _jwtTokenGenerator.GenerateToken(jwtUser);

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

            if (string.IsNullOrWhiteSpace(username))
                return null;

            var dbUser = await _userRepository.GetByUsernameAsync(username);

            if (dbUser == null)
                return null;

            return _mapper.Map<UserProfileDto>(dbUser);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return false;

            return true;
        }

        public async Task UpdateProfileAsync(UpdateProfileRequest request)
        {
            var user = await _userRepository.GetByIdAsync(_userContextService.GetUserId());
            if (user is null)
                throw new Exception("کاربر یافت نشد");

            user.UpdateProfile(request.FullName, request.Email, request.UserName); // متد دامین
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(_userContextService.GetUserId());
            if (user is null)
                throw new Exception("کاربر یافت نشد");


            if (!await ValidatePasswordAsync(request.CurrentPassword, user.UserName))
                throw new Exception("رمز فعلی نادرست است");

            user.ChangePassword(_passwordHasher.HashPassword(user, request.NewPassword)); // متد دامین
            await _userRepository.UpdateAsync(user);
        }
    }
}
