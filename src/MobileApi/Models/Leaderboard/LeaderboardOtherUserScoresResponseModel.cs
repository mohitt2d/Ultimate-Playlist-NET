#region Usings

using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Leaderboard
{
    public class LeaderboardOtherUserScoresResponseModel : BaseResponseModel
    {
        public string AvatarUrl { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public int Amount { get; set; }
    }
}
