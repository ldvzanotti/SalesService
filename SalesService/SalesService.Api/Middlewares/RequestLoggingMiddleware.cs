
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace SalesService.Api.Middlewares
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var watch = Stopwatch.StartNew();

            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Append(CorrelationIdHeader, correlationId);
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers[CorrelationIdHeader] = correlationId;
                    return Task.CompletedTask;
                });
            }

            LogContext.PushProperty(CorrelationIdHeader, correlationId);            

            Log.Information("Incoming Request: {Method} {Path} {QueryString} | {Body} | Correlation ID: {CorrelationId} ",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Request.Body,
                correlationId);

            await next(context);

            watch.Stop();

            Log.Information("Response Status: {StatusCode} | Elapsed Time: {Elapsed} ms | Correlation ID: {CorrelationId}",
                context.Response.StatusCode,
                watch.Elapsed.TotalMilliseconds,
                correlationId);
        }
    }
}
