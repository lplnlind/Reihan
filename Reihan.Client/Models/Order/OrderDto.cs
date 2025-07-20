using Reihan.Client.Enums;

namespace Reihan.Client.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        public int OrderItemCount { get; set; }
    }
}
