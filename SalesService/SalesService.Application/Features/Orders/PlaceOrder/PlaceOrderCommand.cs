using MediatR;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    public record PlaceOrderCommand(NewOrderDto Order) : IRequest<PlaceOrderResponse>;
}
