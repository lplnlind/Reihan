using Application.DTOs;
using Application.Exceptions;
using System;

namespace Reihan.Server.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // ادامه‌ی مسیر request
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Handled AppException: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex, 500, "خطای داخلی سرور رخ داده است.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiErrorDto
            {
                Message = message,
                StatusCode = statusCode,
                ErrorCode = (exception is AppException appEx) ? appEx.ErrorCode.ToString() : "Unknown"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
