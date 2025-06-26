using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        Pending,      // در انتظار تایید
        Processing,   // در حال پردازش
        Shipped,      // ارسال شده
        Delivered,    // تحویل داده شده
        Canceled      // لغو شده
    }
}
