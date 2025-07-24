using Reihan.Shared.DTOs;
using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<UserProfileDto?> GetProfileAsync(ClaimsPrincipal user);
        Task<bool> ValidatePasswordAsync(string password, string userName);
        Task UpdateProfileAsync(UpdateProfileRequest request);
        Task ChangePasswordAsync(ChangePasswordRequest request);
    }
}
