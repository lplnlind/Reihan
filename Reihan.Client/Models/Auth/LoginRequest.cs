using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="لطفا نام کاربری و یا ایمیل خود را وارد کنید")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage ="لطفا رمزعبور خود را بنویسید")]
        public string Password { get; set; } = string.Empty;
    }
}
