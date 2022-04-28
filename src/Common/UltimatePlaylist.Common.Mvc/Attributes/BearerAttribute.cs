#region Usings

using Microsoft.AspNetCore.Authorization;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class BearerAttribute : AuthorizeAttribute
    {
        #region Constructor(s)

        public BearerAttribute()
            : base()
        {
            AuthenticationSchemes = "Bearer";
            Roles = string.Join(',', UserRole.User, UserRole.Administrator);
        }

        public BearerAttribute(string role)
            : base()
        {
            AuthenticationSchemes = "Bearer";
            Roles = role;
        }

        #endregion
    }
}
