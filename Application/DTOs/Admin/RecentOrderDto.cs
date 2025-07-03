namespace Application.DTOs
{
    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string UserFullName { get; set; } = "";
        public decimal Amount { get; set; }
        public string Status { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
