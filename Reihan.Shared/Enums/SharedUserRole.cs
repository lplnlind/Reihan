using System.ComponentModel.DataAnnotations;

namespace Reihan.Shared.Enums
{
    public enum SharedUserRole
    {
        [Display(Name ="ادمین")]
        Admin,

        [Display(Name ="خریدار")]
        Customer
    }
}
