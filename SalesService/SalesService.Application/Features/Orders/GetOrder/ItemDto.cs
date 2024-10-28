namespace SalesService.Application.Features.Orders.GetOrder
{
    public record ItemDto(Guid ProductId, string ProductName, decimal Units);
}
