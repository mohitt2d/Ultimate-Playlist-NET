#region Usings

using Microsoft.AspNetCore.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Identity
{
    public class UserRole : IdentityUserRole<long>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}