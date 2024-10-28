using Marten;
using SalesService.Api.Middlewares;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;
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

            var documentStore = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

            using var session = documentStore.LightweightSession();

            if (!session.Query<Product>().Any())
            {
                session.Store(InitialData.Products);
            }

            if (!session.Query<SalesRepresentative>().Any())
            {
                session.Store(InitialData.SalesRepresentatives);
            }

            if (!session.Query<Order>().Any())
            {
                foreach (var order in InitialData.OrderEvents)
                {
                    session.Events.StartStream<Order>(order.Key, order.Value.OfType<OrderCreated>().Single());
                    session.Events.Append(order.Key, order.Value.Where(e => e.GetType() != typeof(OrderCreated)));
                }
            }

            session.SaveChangesAsync();
        }
    }
}
