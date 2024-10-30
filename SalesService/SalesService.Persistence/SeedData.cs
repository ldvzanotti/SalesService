using Marten;
using Marten.Schema;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;

namespace SalesService.Persistence
{
    public class SeedData() : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = await store.LightweightSerializableSessionAsync(cancellation);

            session.Store(InitialData.Products);
            session.Store(InitialData.SalesRepresentatives);
            
            if (!store.QuerySession().Query<Order>().Any())
            {
                foreach (var order in InitialData.OrderEvents)
                {
                    session.Events.StartStream<Order>(order.Key, order.Value.OfType<OrderCreated>().Single());
                    session.Events.Append(order.Key, order.Value.Where(e => e.GetType() != typeof(OrderCreated)));
                }
            }

            await session.SaveChangesAsync(cancellation);
        }
    }
}
