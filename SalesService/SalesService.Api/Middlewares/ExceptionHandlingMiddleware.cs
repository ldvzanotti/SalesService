using SalesService.Application.Dtos;
using Serilog;
using System.Text.Json;

namespace SalesService.Api.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId);
            Log.Error(exception, "Correlation ID: {CorrelationId}", correlationId);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ApiResponse
            {
                Success = false,
                Message = "Um erro interno impediu o processamento da operação."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
