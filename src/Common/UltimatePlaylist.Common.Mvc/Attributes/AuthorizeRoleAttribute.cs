#region Usings

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(params UserRole[] userRoles)
        {
            Roles = string.Join(',', userRoles.Select(enumValue => enumValue.ToString()));
        }

        public AuthorizeRoleAttribute(string policy, params UserRole[] userRoles)
            : this(userRoles)
        {
            Policy = policy;
        }
    }
}
