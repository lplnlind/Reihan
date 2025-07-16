using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Models
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage ="نام کاربری خود را مشخص کنید")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage ="لطفا نام و نام خانوادگی خود را بنویسید")]
        public string FullName { get; set; } = default!;
        
        [Required(ErrorMessage ="ایمیل خود را بنویسید")]
        public string Email { get; set; } = default!;

        public string? PhoneNumber { get; set; }
    }
}
