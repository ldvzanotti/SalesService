using SalesService.Domain.Abstractions;
using SalesService.Domain.Aggregates.Orders.Events;

namespace SalesService.Domain.Aggregates.Orders
{
    public record Order : IEntity, IAggregateRoot
    {
        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }
        public List<Item> Items { get; set; }
        public Guid SalesRepresentativeId { get; set; }
        public OrderStatus Status { get; set; }

        public Order() { }

        public Order(Guid id, List<Item> items, Guid salesRepresentativeId, OrderStatus orderStatus, DateTime creationDate)
        {
            Id = id;
            Items = items;
            SalesRepresentativeId = salesRepresentativeId;
            CreationDate = creationDate;
            Status = orderStatus;
        }

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

        public Order Apply(OrderStatusUpdated @event) => this with
        {
            Status = @event.NewStatus
        };

        public Order Apply(OrderItemsUpdated @event) => this with
        {
            Items = @event.NewItems
        };
    }
}
