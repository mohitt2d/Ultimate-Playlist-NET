#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Identity
{
    public class GenderEntity : BaseEntity
    {
        #region Constructor(s)

        public GenderEntity()
        {
            Users = new List<User>();
        }

        #endregion

        public string Name { get; set; }

        #region Navigation properties

        public virtual ICollection<User> Users { get; set; }

        #endregion
    }
}
