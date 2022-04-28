#region Usings

using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Mvc.Interface;

#endregion

namespace UltimatePlaylist.Common.Mvc.Middlewares
{
    public class UserLastActiveMiddleware : BaseUserMiddleware
    {
        #region Private properties

        private readonly RequestDelegate Next;

        #endregion

        #region Constructor

        public UserLastActiveMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        #endregion

        #region Invoke

        public async Task InvokeAsync(HttpContext httpContext, IUserLastActiveService userLastActiveService)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                await UpdateLastActiveDate(httpContext, userLastActiveService);
            }

            // Call the next middleware in the pipeline
            await Next(httpContext);
        }

        private async Task UpdateLastActiveDate(HttpContext httpContext, IUserLastActiveService userLastActiveService)
        {
            var userExternalId = GetUserExternalId(httpContext);
            await userLastActiveService.SetLastActiveToUtcNow(userExternalId);
        }

        #endregion
    }
}
