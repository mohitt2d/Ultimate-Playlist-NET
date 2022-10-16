#region Usings

using System.Security.Claims;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public static class JwtClaims
    {
        public const string Id = "Id";

        public const string Role = ClaimTypes.Role;

        public const string Email = ClaimTypes.Email;

        public const string Name = ClaimTypes.NameIdentifier;

        public const string ExternalId = "externalId";

        public const string IsPinRequired = "isPinRequired";
    }
}
