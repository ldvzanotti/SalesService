using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Features.SalesRepresentatives.GetSalesRepresentatives;
using Swashbuckle.AspNetCore.Annotations;

namespace SalesService.Api.Controllers.V1
{
    public class SalesRepresentatives(IMediator mediator) : MainController(mediator)
    {
        [HttpGet()]
        [SwaggerOperation("Listar os representantes de vendas cadastrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Resultado da pesquisa.", Type = typeof(GetSalesRepresentativesResponse))]
        public async Task<IActionResult> GetSalesRepresentatives(
            [SwaggerParameter("Filtrar por CPF:", Required = false), FromQuery(Name = "taxpayer-registration")] string taxpayerRegistration)
        {

            var result = await _mediator.Send(new GetSalesRepresentativesQuery(taxpayerRegistration));
            return Ok(result);
        }
    }
}
