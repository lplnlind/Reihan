using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Enums
{
    public enum OrderStatus
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
