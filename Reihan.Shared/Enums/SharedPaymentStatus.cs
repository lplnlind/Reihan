using System.ComponentModel.DataAnnotations;

namespace Reihan.Shared.Enums
{
    public enum SharedPaymentStatus
    {
        [Display(Name = "در انتظار پرداخت")] 
        Pending,

        [Display(Name = "پرداخت شده")] 
        Completed,

        [Display(Name = "ناموفق")] 
        Failed
    }
}
