using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SalesService.Api.Authentication;
using SalesService.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesService.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController() : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost()]
        [SwaggerOperation("Realizar login.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Token.", Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Erro de autentiação.", Type = typeof(ApiResponse))]
        public IActionResult Login(
            [SwaggerRequestBody("Credenciais:", Required = true), FromBody] Credentials credentials)
        {
            if (credentials.Username == "admin" && credentials.Password == "password1234!")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("D6408A0F-44F2-42F1-84D2-2F26E617A623");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                        new Claim("id", "user-id"),
                        new Claim(JwtRegisteredClaimNames.Sub, credentials.Username)
                    ]),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = "test-dev",
                    Audience = "dev",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized();
        }

        
        [HttpGet()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerOperation("Verificar validade do token.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Token válido.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Token inválido.")]
        public IActionResult ValidateToken()
        {
            return Ok();
        }
    }
}
