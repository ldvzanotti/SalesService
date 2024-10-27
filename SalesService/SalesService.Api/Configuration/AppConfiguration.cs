using SalesService.Api.Middlewares;
using SalesService.Persistence;

namespace SalesService.Api.Configuration
{
    public static class AppConfiguration
    {
        public static void Configure(this WebApplication app)
        {
            app.SeedData();

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

        private static void SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            unitOfWork.SeedData();
        }
    }
}
