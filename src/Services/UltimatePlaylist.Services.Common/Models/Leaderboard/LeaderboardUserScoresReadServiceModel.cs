#region Usings

using UltimatePlaylist.Services.Common.Models.Files;

#endregion
namespace UltimatePlaylist.Services.Common.Models.Leaderboard
{
    public class LeaderboardUserScoresReadServiceModel : BaseReadServiceModel
    {
        public string AvatarUrl { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public long SongRankingPosition { get; set; }

        public long TicketRankingPosition { get; set; }

        public int TicketCount { get; set; }

        public int SongCount { get; set; }
    }
}
