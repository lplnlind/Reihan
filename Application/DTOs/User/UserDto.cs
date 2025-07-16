namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class UpdateUserRoleRequest
    {
        public string NewRole { get; set; } = string.Empty;
    }

    public class ToggleUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
