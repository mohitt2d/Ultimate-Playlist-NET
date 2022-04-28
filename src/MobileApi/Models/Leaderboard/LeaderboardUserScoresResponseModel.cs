#region Usings

using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Leaderboard
{
    public class LeaderboardUserScoresResponseModel : BaseResponseModel
    {
        public string AvatarUrl { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public int SongRankingPosition { get; set; }

        public int TicketRankingPosition { get; set; }

        public int TicketCount { get; set; }

        public int SongCount { get; set; }
    }
}
