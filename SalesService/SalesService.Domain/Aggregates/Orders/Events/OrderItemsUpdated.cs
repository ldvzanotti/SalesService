using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders.Events
{
    public record OrderItemsUpdated(Guid OrderId, List<Item> NewItems) : IEntityEvent;
}
