#region Usings

using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Context;

#endregion

namespace UltimatePlaylist.Common.Mvc.Middlewares
{
    public class EmailChangeWebConfirmationMiddleware : BaseUserMiddleware
    {
        #region Private properties

        private readonly RequestDelegate Next;

        #endregion

        #region Constructor

        public EmailChangeWebConfirmationMiddleware(
            RequestDelegate next)
        {
            Next = next;
        }

        #endregion

        #region Invoke

        public async Task InvokeAsync(
            HttpContext httpContext,
            IUserEmailChangeConfirmedFromWebService userEmailChangeConfirmedFromWeb)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                await CheckIfUserShuldBeBlacklisted(httpContext, userEmailChangeConfirmedFromWeb);
            }

            // Call the next middleware in the pipeline
            await Next(httpContext);
        }

        private async Task CheckIfUserShuldBeBlacklisted(
            HttpContext httpContext,
            IUserEmailChangeConfirmedFromWebService userEmailChangeConfirmedFromWeb)
        {
            var userExternalId = GetUserExternalId(httpContext);
            await userEmailChangeConfirmedFromWeb.CheckIfUserShouldBeLogOut(userExternalId, GetToken(httpContext));
        }

        #endregion
    }
}
