#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Playlist
{
    public class UserPlaylistSongEntity : BaseEntity
    {
        #region Constructor(s)

        public UserPlaylistSongEntity()
        {
            Tickets = new List<TicketEntity>();
        }

        #endregion

        #region Service

        public long? SongId { get; set; }

        public long? UserPlaylistId { get; set; }

        public int Rating { get; set; }

        public bool IsSkipped { get; set; }

        public DateTime? SkipDate { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsFinished { get; set; }

        public int? SecondsListened { get; set; }

        #endregion

        #region Navigation properties

        public virtual UserPlaylistEntity UserPlaylist { get; set; }

        public virtual SongEntity Song { get; set; }

        public virtual ICollection<TicketEntity> Tickets { get; set; }

        #endregion
    }
}
