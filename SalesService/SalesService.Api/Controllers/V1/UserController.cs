using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace SalesService.Api.Controllers.V1
{
    public class UserController(IMediator mediator) : MainController(mediator)
    {
        [HttpGet()]
        [SwaggerOperation("Retorna o nome do usuário autenticado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Nome do usuário autenticado:", Type = typeof(ApiResponse))]
        public IActionResult GetUser()
        {
            var result = new ApiResponse
            {
                Success = true,
                Message = $"Usuário: {User.Identity?.Name}."
            };

            return Ok(result);
        }
    }
}
