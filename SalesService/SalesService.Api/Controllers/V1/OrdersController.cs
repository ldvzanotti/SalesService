using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Features.Orders.GetOrder;
using Swashbuckle.AspNetCore.Annotations;

namespace SalesService.Api.Controllers.V1
{
    public class OrdersController(IMediator mediator) : MainController(mediator)
    {
        [HttpGet("{id}")]
        [SwaggerOperation("Buscar detalhes de um pedido.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Resultado da pesquisa.", Type = typeof(GetOrderDetailsResponse))]
        public async Task<IActionResult> GetOrderDetails(
            [SwaggerParameter("ID do pedido:", Required = true), FromRoute(Name = "id")] Guid id)
        {
            var result = await _mediator.Send(new GetOrderDetailsQuery(id));
            return Ok(result);
        }
    }
}
