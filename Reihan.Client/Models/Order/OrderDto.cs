namespace Reihan.Client.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        public string City { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;

        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        public int OrderItemCount { get; set; }
    }
}
