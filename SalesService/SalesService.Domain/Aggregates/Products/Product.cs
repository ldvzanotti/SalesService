using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Products
{
    public class Product(string name, string barcode, MonetaryValue unitaryPrice, string description = null) : Entity
    {
        public string Name { get; set; } = name;        
        public string Barcode { get; set; } = barcode;
        public MonetaryValue UnitaryPrice { get; set; } = unitaryPrice;
        public string Description { get; set; } = description ?? string.Empty;
    }
}
