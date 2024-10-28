namespace SalesService.Application.Features.Orders
{
    public record ItemDto(Guid ProductId, string ProductName, int Units);
}
