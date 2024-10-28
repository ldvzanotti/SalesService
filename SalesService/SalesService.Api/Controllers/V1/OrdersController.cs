using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Features.Orders.GetOrderDetails;
using SalesService.Application.Features.Orders.PlaceOrder;
using SalesService.Application.Features.Orders.UpdateOrder;
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

        [HttpPost()]
        [SwaggerOperation("Criar um pedido.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Pedido criado.", Type = typeof(PlaceOrderResponse))]
        public async Task<IActionResult> PlaceOrder(
            [SwaggerRequestBody("Dados do pedido:", Required = true), FromBody] NewOrderDto order)
        {
            var result = await _mediator.Send(new PlaceOrderCommand(order));

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetOrderDetails), new { id = result.Order.Id }, result.Order);
        }

        [HttpPatch("{id}")]
        [SwaggerOperation("Atualizar um pedido.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Pedido atualizado.", Type = typeof(UpdateOrderResponse))]
        public async Task<IActionResult> UpdateOrderStatus(
            [SwaggerParameter("ID do pedido:", Required = true), FromRoute] Guid id,
            [SwaggerRequestBody("Dados do pedido", Required = true), FromBody] UpdatedOrderDto order)
        {
            var result = await _mediator.Send(new UpdateOrderCommand(id, order));

            if (!result.Success)
                return BadRequest(result);

            return Ok();
        }
    }
}
