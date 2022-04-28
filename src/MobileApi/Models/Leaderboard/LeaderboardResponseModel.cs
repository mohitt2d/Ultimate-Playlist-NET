namespace UltimatePlaylist.MobileApi.Models.Leaderboard
{
    public class LeaderboardResponseModel
    {
        public LeaderboardUserScoresResponseModel UserStats { get; set; }

        public IList<LeaderboardOtherUserScoresResponseModel> OtherUsersSongListeningRanking { get; set; }

        public IList<LeaderboardOtherUserScoresResponseModel> OtherUsersTicketEarningRanking { get; set; }
    }
}
