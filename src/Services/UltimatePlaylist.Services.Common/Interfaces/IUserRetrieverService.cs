#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Identity
{
    public interface IUserRetrieverService
    {
        string GetUserClaim(string claim);

        bool IsUserInRole(string role);

        bool IsAdmin();

        Guid GetUserExternalId();
    }
}