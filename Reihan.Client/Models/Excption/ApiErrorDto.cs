namespace Reihan.Client.Models
{
    public class ApiErrorDto
    {
        public string Message { get; set; } = default!;
        public int StatusCode { get; set; }
    }
}
