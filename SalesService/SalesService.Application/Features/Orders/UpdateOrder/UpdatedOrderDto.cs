using SalesService.Application.Utils;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Application.Features.Orders.UpdateOrder
{
    public record UpdatedOrderDto
    {
        [AllowedOrderStatus]
        public string Status { get; set; }

        [MinLength(1, ErrorMessage = "Número de itens deve ser maior que 0.")]
        public List<ItemDto> Items { get; set; }

        public UpdatedOrderDto(string status, List<ItemDto> items)
        {
            Status = status;
            Items = items.GroupBy(p => p.ProductId).Select(g => new ItemDto(g.Key, g.Sum(g => g.Units))).Where(p => p.Units > 0).ToList();
        }
    }
}
