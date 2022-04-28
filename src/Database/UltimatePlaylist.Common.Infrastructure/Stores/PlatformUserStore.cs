#region Usings

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UltimatePlaylist.Database.Infrastructure.Context;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Stores
{
    public class PlatformUserStore : UserStore<User, Role, EFContext, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityUserToken<long>, IdentityRoleClaim<long>>
    {
        public PlatformUserStore(EFContext context, IdentityErrorDescriber describer)
            : base(context, describer)
        {
        }
    }
}
