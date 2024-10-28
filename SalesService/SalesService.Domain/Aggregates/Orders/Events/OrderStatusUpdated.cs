using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders.Events
{
    public record OrderStatusUpdated(Guid OrderId, OrderStatus NewStatus) : IEntityEvent;
}
