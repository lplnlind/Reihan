namespace Application.DTOs.Auth
{
    public class UpdateProfileRequest
    {
        public string UserName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
