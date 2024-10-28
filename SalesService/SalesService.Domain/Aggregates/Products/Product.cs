using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Products
{
    public record Product : IEntity, IAggregateRoot
    {
        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public MonetaryValue UnitaryPrice { get; set; }
        public string Description { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Product(string name, string barcode, MonetaryValue unitaryPrice, string description = null) : this()
        {
            Name = name;
            Barcode = barcode;
            UnitaryPrice = unitaryPrice;
            Description = description ?? string.Empty;
        }

        public Product(Guid id, DateTime creationDate, string name, string barcode, MonetaryValue unitaryPrice, string description = null)
            : this(name, barcode, unitaryPrice, description)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
