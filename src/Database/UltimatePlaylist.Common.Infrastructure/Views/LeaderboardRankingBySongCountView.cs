#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Base;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Views
{
    public class LeaderboardRankingBySongCountView : BaseEntity
    {
        public long RankingPosition { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public int SongCount { get; set; }
    }
}
