using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IAuthClient
    {
        Task<bool> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
        Task<UserProfileDto> GetProfileAsync();
        Task UpdateProfileAsync(UpdateProfileRequest request);
        Task ChangePasswordAsync(ChangePasswordRequest request);
        Task<JwtUserDto?> GetCurrentUserAsync();
    }
}
