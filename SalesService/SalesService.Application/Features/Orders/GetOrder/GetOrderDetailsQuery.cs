using MediatR;

namespace SalesService.Application.Features.Orders.GetOrder
{
    public record GetOrderDetailsQuery(Guid OrderId) : IRequest<GetOrderDetailsResponse>;
}
