#region Usings

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Identity
{
    public class Role : IdentityRole<long>
    {
        public virtual ICollection<UserRole> Users { get; set; }
    }
}