namespace Reihan.Client.Models.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        public decimal TotalAmount { get; set; }
    }
}
