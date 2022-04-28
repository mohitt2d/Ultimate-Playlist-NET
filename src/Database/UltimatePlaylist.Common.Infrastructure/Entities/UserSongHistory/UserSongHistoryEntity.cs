#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory
{
    public class UserSongHistoryEntity : BaseEntity
    {
        #region Constructor(s)

        public UserSongHistoryEntity()
        {
            Tickets = new List<TicketEntity>();
        }

        #endregion

        #region Service

        public long? SongId { get; set; }

        public long? UserId { get; set; }

        public bool IsAddedToSpotify { get; set; }

        public bool IsAddedToAppleMusic { get; set; }

        #endregion

        #region Navigation properties

        public virtual SongEntity Song { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<TicketEntity> Tickets { get; set; }

        #endregion
    }
}
