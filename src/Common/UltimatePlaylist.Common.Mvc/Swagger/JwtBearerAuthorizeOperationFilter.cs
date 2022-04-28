#region Usings

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace UltimatePlaylist.Common.Mvc.Swagger
{
    public class JwtBearerAuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerType = context.MethodInfo.DeclaringType;
            var methodInfo = context.MethodInfo;

            var controllerAttributes = controllerType.GetCustomAttributes(inherit: true);
            var methodAttributes = methodInfo.GetCustomAttributes(inherit: true);

            var controllerAuthAttributes = controllerAttributes.OfType<AuthorizeAttribute>();
            var methodAuthAttributes = methodAttributes.OfType<AuthorizeAttribute>();

            // Has no authorize attributes?
            if (!controllerAuthAttributes.Any() && !methodAuthAttributes.Any())
            {
                return;
            }

            var controllerAllowAnonymousAttributes = controllerAttributes.OfType<AllowAnonymousAttribute>();
            var methodAllowAnonymousAttributes = methodAttributes.OfType<AllowAnonymousAttribute>();

            // Has any allow anonymous attributes overriding authorization requirement?
            if (controllerAllowAnonymousAttributes.Any() || methodAllowAnonymousAttributes.Any())
            {
                return;
            }

            var securityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme,
                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
            };

            operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
        }
    }
}
