using Domain.Enums;

namespace Application.DTOs
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        public OrderStatus Status { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
