#region Usings

using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Services.Common.Interfaces.Identity;

#endregion

namespace UltimatePlaylist.Services.Identity.Services
{
    public class UserRetrieverService : IUserRetrieverService
    {
        #region Private Members

        private readonly HttpContext HttpContext;

        #endregion

        #region Constructor

        public UserRetrieverService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        public string GetUserClaim(string claim)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == claim)?.Value;
        }

        public bool IsUserInRole(string role)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            return HttpContext.User.IsInRole(role);
        }

        public bool IsAdmin()
        {
            return IsUserInRole(UserRole.Administrator.ToString());
        }

        public Guid GetUserExternalId()
        {
            var userIdClaim = GetUserClaim(JwtClaims.ExternalId);
            var userEmailClaim = GetUserClaim(JwtClaims.Email);
            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(userEmailClaim))
            {
                throw new LoginException(ErrorType.Unauthorized);
            }

            return new Guid(userIdClaim).Decode(userEmailClaim);
        }
    }
}
