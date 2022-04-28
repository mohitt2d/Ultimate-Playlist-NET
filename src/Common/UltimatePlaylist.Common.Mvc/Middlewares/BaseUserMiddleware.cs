#region Usings

using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Common.Extensions;

#endregion

namespace UltimatePlaylist.Common.Mvc.Middlewares
{
    public class BaseUserMiddleware
    {
        protected Guid GetUserExternalId(HttpContext httpContext)
        {
            var userEmail = httpContext.User.Claims?.FirstOrDefault(c => c.Type.Equals(JwtClaims.Email))?.Value;
            var parseResult = Guid.TryParse(httpContext.User.Claims?.FirstOrDefault(c => c.Type.Equals(JwtClaims.ExternalId))?.Value, out Guid externalId);

            if (string.IsNullOrEmpty(userEmail) || !parseResult)
            {
                throw new NotFoundException("UserId Not Found");
            }

            return externalId.Decode(userEmail);
        }

        protected string GetToken(HttpContext httpContext)
        {
            return httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
        }
    }
}
