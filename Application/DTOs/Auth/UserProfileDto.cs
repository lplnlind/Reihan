using Domain.Enums;

namespace Application.DTOs.Auth
{
    public class UserProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
    }
}
