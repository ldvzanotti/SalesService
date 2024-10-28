using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    internal record PlaceOrderData(SalesRepresentative SalesRepresentative, IReadOnlyList<Product> Products);
}
