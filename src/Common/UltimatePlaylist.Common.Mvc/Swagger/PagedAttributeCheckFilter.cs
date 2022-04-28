#region Usings

using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Paging;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.Common.Mvc.Swagger
{
    public class PagedAttributeCheckFilter : IOperationFilter
    {
        #region Apply

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var info);
            var attrs = info.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(ProducesEnvelopeAttribute))?.ConstructorArguments;
            var type = attrs?[0].Value?.ToString().Split("[")?[0];

            if (type == typeof(FilteredResponse<>).FullName
                || type == typeof(PaginatedResponse<>).FullName)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<OpenApiParameter>();
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = XParameters.Query,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = false,
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = XParameters.Order,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = false,
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = XParameters.Desc,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "boolean" },
                    Required = false,
                });
            }

            if (type == typeof(PaginatedResponse<>).FullName)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = XParameters.PageNumber,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "integer" },
                    Required = false,
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = XParameters.PageSize,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "integer" },
                    Required = false,
                });
            }
        }

        #endregion
    }
}