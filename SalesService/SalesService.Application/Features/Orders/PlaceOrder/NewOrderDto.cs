using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    public record NewOrderDto
    {
        [Required(ErrorMessage = "Deve ser informado o ID do representante de vendas.")]
        public Guid SalesRepresentativeId { get; set; }

        [MinLength(1, ErrorMessage = "Número de itens deve ser maior que 0.")]
        public List<CartItemDto> Items { get; set; }

        public NewOrderDto(Guid salesRepresentativeId, List<CartItemDto> items)
        {
            SalesRepresentativeId = salesRepresentativeId;
            Items = items.GroupBy(p => p.ProductId).Select(g => new CartItemDto(g.Key, g.Sum(g => g.Units))).Where(p => p.Units > 0).ToList();
        }
    }
}
