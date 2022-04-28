#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File
{
    public class AvatarFileEntity : BaseFileEntity
    {
        #region Properties

        public long? UserId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual User User { get; set; }

        #endregion
    }
}
