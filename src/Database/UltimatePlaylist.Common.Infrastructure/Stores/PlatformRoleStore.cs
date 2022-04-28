#region Usings

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UltimatePlaylist.Database.Infrastructure.Context;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Stores
{
    public class PlatformRoleStore : RoleStore<Role, EFContext, long, UserRole, IdentityRoleClaim<long>>
    {
        public PlatformRoleStore(EFContext context)
            : base(context)
        {
        }
    }
}
