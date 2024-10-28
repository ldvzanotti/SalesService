using System.ComponentModel.DataAnnotations;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    public record CartItemDto
    {
        [Required(ErrorMessage = "Deve ser informado o ID do produto.")]
        public Guid ProductId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Número de unidades deve ser maior que 0.")]
        public int Units { get; set; }

        public CartItemDto(Guid productId, int units)
        {
            ProductId = productId;
            Units = units;
        }
    }
}
