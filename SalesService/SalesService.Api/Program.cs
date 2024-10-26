using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SalesService.Api.Configuration;
using ShoppingService.Application.Dtos;
using System.Text.Json;

namespace SalesService.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

            builder.Services.AddResponseCompression();

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

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

            var app = builder.Build();

            app.UseSwaggerConfiguration();

            app.UseHttpsRedirection();

            app.UseCors("Open");

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCompression();

            app.MapGet("/", () => "App online!");

            app.MapControllerRoute("default", "/[controller]")
                .RequireAuthorization();

            await app.RunAsync();
        }
    }
}
