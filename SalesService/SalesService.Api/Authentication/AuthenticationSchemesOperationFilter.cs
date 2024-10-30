using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SalesService.Api.Authentication
{
    public class AuthenticationSchemesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context is not null && operation is not null)
            {
                var classAuthSchemes = context.MethodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<AuthorizeAttribute>()
                        .Select(attr => attr.AuthenticationSchemes)
                        .Distinct();

                var methodAuthSchemes = context.MethodInfo
                        .GetCustomAttributes(true)
                        .OfType<AuthorizeAttribute>()
                        .Select(attr => attr.AuthenticationSchemes)
                        .Distinct();


                var requiresAuthentication = classAuthSchemes.Contains(JwtBearerDefaults.AuthenticationScheme) ||
                    methodAuthSchemes.Contains(JwtBearerDefaults.AuthenticationScheme);

                if (!requiresAuthentication)
                    return;

                operation.Responses.TryAdd(StatusCodes.Status401Unauthorized.ToString(),
                    new OpenApiResponse { Description = "Usuário não autenticado." });

                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                ];
            }
        }
    }
}
