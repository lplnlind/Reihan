using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "در انتظار تایید")]
        Pending, 
        
        [Display(Name = "در حال پردازش")]
        Processing,  
        
        [Display(Name = "ارسال شده")]
        Shipped,     
        
        [Display(Name = "تحویل داده شده")]
        Delivered,   
        
        [Display(Name = "لغو شده")]
        Canceled      
    }
}
