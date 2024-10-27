using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders
{
    public class Order(List<Item> items, Guid salesRepresentativeId) : Entity, IAggregateRoot
    {
        public List<Item> Items { get; set; } = items;
        public Guid SalesRepresentativeId { get; set; } = salesRepresentativeId;
        public OrderStatus Status { get; set; } = OrderStatus.PaymentPending;

        public void UpdateStatus(OrderStatus newStatus) => Status = newStatus;

        public void UpdateItems(List<Item> items) => Items = items;
    }
}
