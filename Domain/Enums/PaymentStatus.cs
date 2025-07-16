using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "در انتظار پرداخت")] 
        Pending,

        [Display(Name = "پرداخت شده")] 
        Completed,

        [Display(Name = "ناموفق")] 
        Failed
    }
}
