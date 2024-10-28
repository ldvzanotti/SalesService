using SalesService.Application.Dtos;
using System.Text.Json.Serialization;

namespace SalesService.Application.Features.Orders.GetOrderDetails
{
    public record GetOrderDetailsResponse : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderDto Order { get; set; }

        public GetOrderDetailsResponse() { }
    }
}
