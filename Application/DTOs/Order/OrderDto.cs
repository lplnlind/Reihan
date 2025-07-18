using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        public int OrderItemCount { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }
}
