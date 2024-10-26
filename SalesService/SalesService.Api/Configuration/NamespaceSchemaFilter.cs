using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SalesService.Api.Configuration
{
    public class NamespaceSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(schema);

            ArgumentNullException.ThrowIfNull(context);

            schema.Title = context.Type.FullName[(context.Type.FullName[..context.Type.FullName.LastIndexOf('.')].LastIndexOf('.') + 1)..];
        }
    }
}
