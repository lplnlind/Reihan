namespace Reihan.Client.Pages.Common
{
    public class OrderSummaryModel
    {
        public decimal TotalWithoutDiscount { get; set; }
        public decimal ShippingCost { get; set; } = 50000;
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmount => TotalWithoutDiscount + ShippingCost - TotalDiscount;
    }
}
