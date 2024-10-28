namespace SalesService.Domain.Aggregates.Orders
{
    public record Item(Guid ProductId, decimal Units);
}
