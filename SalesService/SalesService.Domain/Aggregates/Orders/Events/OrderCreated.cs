using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders.Events
{
    public record OrderCreated : IEntityEvent
    {
        public List<Item> Items { get; set; }
        public Guid SalesRepresentativeId { get; set; }
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }

        public OrderCreated(List<Item> items, Guid salesRepresentativeId, Guid? id = null, DateTime? creationDate = null)
        {
            Items = items;
            SalesRepresentativeId = salesRepresentativeId;
            Id = id ?? Guid.NewGuid();
            CreationDate = creationDate ?? DateTime.UtcNow;
        }
    }
}
