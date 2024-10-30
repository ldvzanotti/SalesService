using SalesService.Application.Utils;

namespace SalesService.Application.Features.Orders.UpdateOrder
{
    public record UpdateOrderDto
    {
        [AllowedOrderStatus]
        public string Status { get; set; }

        [MinLengthNotRequired(1)]
        public List<ItemDto> Items { get; set; }

        public UpdateOrderDto(string status, List<ItemDto> items)
        {
            Status = status;
            Items = items?.GroupBy(p => p.ProductId).Select(g => new ItemDto(g.Key, g.Sum(g => g.Units))).Where(p => p.Units > 0).ToList();
        }
    }
}
