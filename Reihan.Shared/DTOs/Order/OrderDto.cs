using Reihan.Shared.Enums;

namespace Reihan.Shared.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public SharedOrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        public int OrderItemCount { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public SharedOrderStatus NewStatus { get; set; }
    }
}
