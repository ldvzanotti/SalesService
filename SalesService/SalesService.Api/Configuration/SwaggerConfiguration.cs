using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace SalesService.Api.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping Service", Version = "v1", });

                options.CustomSchemaIds(x => x.FullName);
                options.SchemaFilter<NamespaceSchemaFilter>();
                options.EnableAnnotations();

                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<AuthenticationSchemesOperationFilter>();

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Bearer token authentication",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger(c => c.PreSerializeFilters.Add((swagger, _) =>
            {
                if (swagger.Components != null && swagger.Components.Schemas != null)
                {
                    var replacement = new Dictionary<string, OpenApiSchema>();
                    foreach (var kv in swagger.Components.Schemas.OrderBy(p => p.Key))
                    {
                        replacement.Add(kv.Key, kv.Value);
                    }
                    swagger.Components.Schemas = replacement;
                }
            }));

            app.UseSwaggerUI(options =>
            {
                options.EnableTryItOutByDefault();
                options.DisplayRequestDuration();
                options.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
