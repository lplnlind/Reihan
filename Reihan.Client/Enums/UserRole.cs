using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Enums
{
    public enum UserRole
    {
        [Display(Name ="ادمین")]
        Admin,

        [Display(Name ="خریدار")]
        Customer
    }
}
