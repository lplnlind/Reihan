using Domain.Enums;

namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateUserRoleRequest
    {
        public UserRole NewRole { get; set; }
    }

    public class ToggleUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
