using SalesService.Application.Dtos;
using System.Text.Json.Serialization;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    public record PlaceOrderResponse : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderDto Order { get; set; }
    }
}
