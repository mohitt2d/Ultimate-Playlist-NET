#region Usings

using System;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Dsp
{
    public class UserDspEntity : BaseEntity
    {
        #region Constructor

        public UserDspEntity()
        {
            Created = DateTime.UtcNow;
        }

        #endregion

        #region Service

        public DspType Type { get; set; }

        public bool IsActive { get; set; }

        public string SpotifyAccessToken { get; set; }

        public string SpotifyRefreshToken { get; set; }

        public string SpotifyTokenType { get; set; }

        public string SpotifyScopes { get; set; }

        public string AppleUserToken { get; set; }

        public DateTime? AccessTokenExpirationDate { get; set; }

        public long? UserId { get; set; }

        public string UserSpotifyIdentity { get; set; }

        public string UserPlaylistId { get; set; }

        #endregion

        #region Navigation properties

        public virtual User User { get; set; }

        #endregion
    }
}
