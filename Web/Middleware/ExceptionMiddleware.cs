using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleCustomExceptionResponseAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionResponseAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = ex switch
            {
                ArgumentNullException _ => (int)HttpStatusCode.BadRequest,
                ArgumentException _ => (int)HttpStatusCode.BadRequest,
                InvalidOperationException _ => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException _ => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new ErrorModel(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
        public class ErrorModel
        {
            public int Code { get; set; }
            public string? Message { get; set; }
            public string? Stack { get; set; }

            public ErrorModel(int code, string? message, string? stack = null)
            {
                Code = code;
                Message = message;
                Stack = stack;
            }

            public ErrorModel(int code, IDictionary<string, string[]> message, string? stack)
            {
                Code = code;
                Message = string.Join(", ", message.Values.SelectMany(v => v).Distinct());
                Stack = stack;
            }
        }
    }
}