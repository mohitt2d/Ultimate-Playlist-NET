#region Usings

using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Mvc.Interface;

#endregion

namespace UltimatePlaylist.Common.Mvc.Middlewares
{
    public class UserTokenMiddleware : BaseUserMiddleware
    {
        #region Private properties

        private readonly RequestDelegate Next;

        #endregion

        #region Constructor

        public UserTokenMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        #endregion

        #region Invoke

        public async Task InvokeAsync(HttpContext httpContext, IUserActiveService userActiveService)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                await CheckActiveStatus(httpContext, userActiveService);
            }

            // Call the next middleware in the pipeline
            await Next(httpContext);
        }

        private async Task CheckActiveStatus(HttpContext httpContext, IUserActiveService userActiveService)
        {
            var userExternalId = GetUserExternalId(httpContext);
            var token = GetToken(httpContext);
            await userActiveService.CheckActiveStatusAndSetTokenOnBlacklist(userExternalId, token);
        }

        #endregion
    }
}
