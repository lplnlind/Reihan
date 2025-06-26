namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public bool IsActive { get; set; }
    }

    public class UpdateUserRoleRequest
    {
        public string NewRole { get; set; } = "";
    }

    public class ToggleUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
