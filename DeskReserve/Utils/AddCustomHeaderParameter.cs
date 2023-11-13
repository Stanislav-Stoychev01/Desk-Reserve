using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeskReserve.Utils
{
    public class AddCustomHeaderParameter
        : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            if (operation.Parameters is null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Custom Header",
                In = ParameterLocation.Header,
                Description = "Custom Header description",
                Required = true,
            });
        }
    }
}
