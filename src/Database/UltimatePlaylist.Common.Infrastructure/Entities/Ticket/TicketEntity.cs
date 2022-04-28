#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Ticket
{
    public class TicketEntity : BaseEntity
    {
        #region Service

        public TicketType Type { get; set; }

        public TicketEarnedType EarnedType { get; set; }

        public bool IsUsed { get; set; }

        #endregion

        #region Navigation Properties

        public long? UserSongHistoryId { get; set; }

        public virtual UserSongHistoryEntity UserSongHistory { get; set; }

        public long? UserPlaylistSongId { get; set; }

        public virtual UserPlaylistSongEntity UserPlaylistSong { get; set; }

        #endregion
    }
}
