using SalesService.Api.Configuration;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;

namespace SalesService.Api
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    rollOnFileSizeLimit: true
                )
                .Enrich.WithSensitiveDataMasking(options =>
                {
                    options.MaskValue = "*** PII HIDDEN ***";
                    options.MaskProperties = ["Authorization", "Cookie", "Set-Cookie"];
                    options.Mode = MaskingMode.Globally;
                })
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.ConfigureServices();

            var app = builder.Build();

            app.Configure();

            await app.RunAsync();
        }
    }

    public partial class Program
    {

    }
}
