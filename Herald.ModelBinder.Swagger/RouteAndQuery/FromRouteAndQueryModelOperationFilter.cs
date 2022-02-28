using Herald.ModelBinder.RouteAndQuery;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Linq;

namespace Herald.ModelBinder.Swagger
{
    public class FromRouteAndQueryModelOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var parameterDescriptions = context.ApiDescription.ParameterDescriptions
                .Where(x => x.Source.DisplayName == nameof(FromRouteAndQueryAttribute))
                .Join(context.ApiDescription.ParameterDescriptions, c => c.Name.ToUpper(), c2 => c2.Name.ToUpper(), (c, c2) => new { c, c2 })
                .Where(x => x.c != x.c2)
                .Select(x => x.c)
                .ToArray();

            foreach (var parameterDescription in parameterDescriptions)
            {
                if (context.ApiDescription.ParameterDescriptions.Contains(parameterDescription))
                {
                    context.ApiDescription.ParameterDescriptions.Remove(parameterDescription);
                }

                var parameter = operation.Parameters.FirstOrDefault(p => p.Name == parameterDescription.Name);
                if (parameter != null)
                {
                    operation.Parameters.Remove(parameter);
                }
            }
        }
    }
}