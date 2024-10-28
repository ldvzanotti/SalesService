using MediatR;

namespace SalesService.Application.Features.Orders.GetOrderDetails
{
    public record GetOrderDetailsQuery(Guid OrderId) : IRequest<GetOrderDetailsResponse>;
}
