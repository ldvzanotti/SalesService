namespace SalesService.Application.Features.Orders
{
    public record OrderDto(Guid Id, DateTime CreationDate, string Status, SalesRepresentativeDto SalesRepresentative, List<ItemDto> Items);
}
