using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Application.Features.Orders.GetOrderDetails
{
    internal record GetOrderDetailsData(Order Order, SalesRepresentative SalesRepresentative, IReadOnlyList<Product> Products);
}
