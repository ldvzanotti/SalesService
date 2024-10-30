using SalesService.Api.Middlewares;

namespace SalesService.Api.Configuration
{
    public static class AppConfiguration
    {
        public static void Configure(this WebApplication app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSwaggerConfiguration();

            app.UseHttpsRedirection();

            app.UseCors("Open");

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCompression();

            app.MapGet("/", () => "App online!");

            app.MapControllerRoute("default", "/[controller]")
                .RequireAuthorization();
        }
    }
}
