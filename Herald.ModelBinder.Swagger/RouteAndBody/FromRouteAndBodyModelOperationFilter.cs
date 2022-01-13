using Herald.ModelBinder.RouteAndBody;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Herald.ModelBinder.Swagger
{
    public class FromRouteAndBodyModelOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var description = context.ApiDescription;

            var actionParameters = description.ActionDescriptor.Parameters.OfType<ControllerParameterDescriptor>()
                .Where(p => p.ParameterInfo.CustomAttributes.Any(a => a.AttributeType == typeof(FromRouteAndBodyAttribute)))
                .ToList();

            if (!actionParameters.Any())
                return;

            var apiParameters = description.ParameterDescriptions
                .Where(p => p.Source.IsFromRequest)
                .ToList();

            WalkOperation(operation, context, actionParameters, apiParameters);
        }

        private static void WalkOperation(OpenApiOperation operation,
            OperationFilterContext context,
            IReadOnlyCollection<ControllerParameterDescriptor> actionParameters,
            IReadOnlyCollection<ApiParameterDescription> apiParameters)
        {
            foreach (var actionParameter in actionParameters.IntersectBy(context.SchemaRepository.Schemas.Keys, x => x.ParameterType.Name, StringComparer.OrdinalIgnoreCase))
            {
                foreach (var property in context
                             .SchemaRepository.Schemas[actionParameter.ParameterType.Name]
                             .Properties.IntersectBy(apiParameters.Select(x => x.Name), x => x.Key,
                                 StringComparer.OrdinalIgnoreCase))
                {

                    context.SchemaRepository.Schemas[actionParameter.ParameterType.Name].Properties.Remove(property);
                    var parameter = operation.Parameters.FirstOrDefault(p => string.Equals(p.Name, actionParameter.Name, StringComparison.OrdinalIgnoreCase));
                    if (parameter != null) operation.Parameters.Remove(parameter);
                }

                if (!context.SchemaRepository.Schemas[actionParameter.ParameterType.Name].Properties.Any())
                {
                    operation.RequestBody = null;
                    continue;
                }

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(actionParameter.ParameterType, context.SchemaRepository)
                    }
                }
                };
            }
        }
    }
}