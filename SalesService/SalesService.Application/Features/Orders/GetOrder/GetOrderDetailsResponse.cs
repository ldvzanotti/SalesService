using SalesService.Application.Dtos;
using System.Text.Json.Serialization;

namespace SalesService.Application.Features.Orders.GetOrder
{
    public record GetOrderDetailsResponse : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderDto Order { get; set; }

        public GetOrderDetailsResponse() { }
    }
}
