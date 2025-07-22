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
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage ="لطفا شماره موبایل خود را وارد کنید")]
        [Phone(ErrorMessage = "شماره تلفن معتبر نیست")]
        public string PhoneNumber { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}
