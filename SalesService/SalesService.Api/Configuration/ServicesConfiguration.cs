using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using SalesService.Api.Middlewares;
using SalesService.Application.Dtos;
using SalesService.Application.Features.SalesRepresentatives.GetSalesRepresentatives;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;
using SalesService.Persistence;
using System.Text;
using System.Text.Json;
using Weasel.Core;

namespace SalesService.Api.Configuration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<RequestLoggingMiddleware>();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection"));

            builder.Services.AddMarten(options =>
            {
                options.DatabaseSchemaName = "dev";
                options.AutoCreateSchemaObjects = AutoCreate.All;

                options.UseSystemTextJsonForSerialization();

                options.Projections.Add<OrderProjection>(ProjectionLifecycle.Inline);

                options.ApplicationAssembly = typeof(OrderCreated).Assembly;
            })
                .InitializeWith(new SeedData())
                .UseNpgsqlDataSource()
                .AddAsyncDaemon(DaemonMode.Solo);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetSalesRepresentativesQuery>());

            builder.Services.AddResponseCompression();

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = "dev",
                        ValidateIssuer = true,
                        ValidIssuer = "test-dev",
                        RequireExpirationTime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("D6408A0F-44F2-42F1-84D2-2F26E617A623"))
                    };
                });
            }

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
                        Message = "Ocorreram erros durante o processamento da operação.",
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
