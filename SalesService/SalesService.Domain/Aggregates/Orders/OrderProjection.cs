using Marten.Events.Aggregation;
using SalesService.Domain.Aggregates.Orders.Events;

namespace SalesService.Domain.Aggregates.Orders
{
    public class OrderProjection : SingleStreamProjection<Order>
    {
        public static Order Create(OrderCreated @event)
        {
            return new Order(
                @event.Id,
                @event.Items,
                @event.SalesRepresentativeId,
                OrderStatus.PaymentPending,
                @event.CreationDate
            );
        }

        public static void Apply(OrderStatusUpdated @event, Order order) => order.Status = @event.NewStatus;
        public static void Apply(OrderItemsUpdated @event, Order order) => order.Items = @event.NewItems;
    }
}
