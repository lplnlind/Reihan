namespace Application.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; }
        public ErrorCode ErrorCode { get; }

        public AppException(string message, int statusCode = 400, ErrorCode errorCode = default)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
