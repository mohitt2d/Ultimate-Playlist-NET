#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Views
{
    public class SongPopularityView : BaseEntity
    {
        public long Position { get; set; }

        public int Amount { get; set; }
    }
}
