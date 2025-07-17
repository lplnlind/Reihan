namespace Application.DTOs
{
    public class ApiErrorDto
    {
        public string Message { get; set; } = default!;
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; } = "Unknown";
    }
}
