using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace FlightDocsSystem.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                // Đặt schemaId duy nhất dựa trên kiểu enum
                schema.Enum = Enum.GetNames(context.Type)
                                  .Select(name => new OpenApiString(name))
                                  .Cast<IOpenApiAny>()
                                  .ToList();
                schema.Title = context.Type.Name; // Đặt tiêu đề để Swagger dễ hiểu
            }
        }
    }
}
