using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders
{
    public class Item(Guid productId, decimal units) : Entity
    {
        public Guid ProductId { get; set; } = productId;
        public decimal Units { get; set; } = units;
    }
}
