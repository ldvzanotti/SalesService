using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SalesService.Api.Middlewares;
using SalesService.Application.Dtos;
using System.Text.Json;

namespace SalesService.Api.Configuration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<RequestLoggingMiddleware>();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddControllers();

            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

            builder.Services.AddResponseCompression();

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            builder.Services.AddVersioning();

            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            });

            builder.Services.Configure<JsonSerializerOptions>(options =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.WriteIndented = true;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Values.SelectMany(e => e.Errors);
                    var errorMsgs = errors.Select(e => e.Exception?.Message ?? e.ErrorMessage).ToList();
                    var response = new ApiResponse
                    {
                        Success = false,
                        Message = "Ocorreram erros durante o processamento da operação:",
                        Errors = errorMsgs
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }
    }
}
