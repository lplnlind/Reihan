using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage ="نام کاربری را وارد کنید.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage ="نام و نام خانوادگی خود را بنویسید.")]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage ="ایمیل خود را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="لطفا شماره موبایل خود را وارد کنید")]
        [Phone(ErrorMessage = "شماره تلفن معتبر نیست")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage ="گذرواژه خود را مشخص کنید.")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage ="تکرار گذرواژه را بنویسید.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
