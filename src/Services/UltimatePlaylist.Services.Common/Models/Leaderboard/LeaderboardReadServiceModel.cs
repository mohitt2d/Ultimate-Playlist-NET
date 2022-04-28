namespace UltimatePlaylist.Services.Common.Models.Leaderboard
{
    public class LeaderboardReadServiceModel
    {
        public LeaderboardUserScoresReadServiceModel UserStats { get; set; }

        public IList<LeaderboardOtherUserScoresReadServiceModel> OtherUsersSongListeningRanking { get; set; }

        public IList<LeaderboardOtherUserScoresReadServiceModel> OtherUsersTicketEarningRanking { get; set; }
    }
}
