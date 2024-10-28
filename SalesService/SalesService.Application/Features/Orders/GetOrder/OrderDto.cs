namespace SalesService.Application.Features.Orders.GetOrder
{
    public record OrderDto(Guid Id, DateTime CreationDate, string Status, SalesRepresentativeDto SalesRepresentative, List<ItemDto> Items);
}
