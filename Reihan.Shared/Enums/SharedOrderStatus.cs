using System.ComponentModel.DataAnnotations;

namespace Reihan.Shared.Enums
{
    public enum SharedOrderStatus
    {
        [Display(Name = "در انتظار تایید")]
        Pending, 
        
        [Display(Name = "درحال آماده سازی")]
        Processing,  
        
        [Display(Name = "ارسال شده")]
        Shipped,     
        
        [Display(Name = "تحویل داده شده")]
        Delivered,   
        
        [Display(Name = "لغو شده")]
        Canceled      
    }
}
