using Reihan.Client.Enums;

namespace Reihan.Client.Models
{
    public class UserProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
