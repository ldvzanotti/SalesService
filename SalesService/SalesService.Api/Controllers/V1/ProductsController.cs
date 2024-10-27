using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Features.Products.GetProducts;
using Swashbuckle.AspNetCore.Annotations;

namespace SalesService.Api.Controllers.V1
{
    public class ProductsController(IMediator mediator) : MainController(mediator)
    {
        [HttpGet()]
        [SwaggerOperation("Listar os produtos cadastrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Resultado da pesquisa.", Type = typeof(GetProductsResponse))]
        public async Task<IActionResult> GetProducts(
            [SwaggerParameter("Filtrar por código de barras:", Required = false), FromQuery(Name = "barcode")] string barcode)
        {

            var result = await _mediator.Send(new GetProductsQuery(barcode));
            return Ok(result);
        }
    }
}
