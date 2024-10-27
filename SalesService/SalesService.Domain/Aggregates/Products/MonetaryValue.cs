using SalesService.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Domain.Aggregates.Products
{
    public class MonetaryValue(decimal value, string currency) : ValueObject
    {
        public decimal Value { get; init; } = value;

        [MaxLength(3)]
        public string Currency { get; init; } = currency;

        public override string ToString() => $"{Currency} {Value}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }
}