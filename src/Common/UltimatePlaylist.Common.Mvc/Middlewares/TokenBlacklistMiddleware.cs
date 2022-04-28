#region Usings

using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Mvc.Interface;

#endregion

namespace UltimatePlaylist.Common.Mvc.Middlewares
{
    public class TokenBlacklistMiddleware : BaseUserMiddleware
    {
        #region Private properties

        private readonly RequestDelegate Next;

        #endregion

        #region Constructor

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        #endregion

        #region Invoke

        public async Task InvokeAsync(HttpContext httpContext, IUserBlacklistTokenStore blacklistTokenStore)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                CheckBlacklistedToken(httpContext, blacklistTokenStore);
            }

            // Call the next middleware in the pipeline
            await Next(httpContext);
        }

        private void CheckBlacklistedToken(HttpContext httpContext, IUserBlacklistTokenStore blacklistTokenStore)
        {
            var userExternalId = GetUserExternalId(httpContext);
            if (blacklistTokenStore.TryGet(userExternalId, out string savedToken))
            {
                if (GetToken(httpContext).Equals(savedToken))
                {
                    throw new UnauthorizedAccessException("User is unauthorized");
                }
            }
        }

        #endregion
    }
}
