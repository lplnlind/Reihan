using Reihan.Shared.Enums;

namespace Reihan.Shared.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public SharedUserRole Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateUserRoleRequest
    {
        public SharedUserRole NewRole { get; set; }
    }

    public class ToggleUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
