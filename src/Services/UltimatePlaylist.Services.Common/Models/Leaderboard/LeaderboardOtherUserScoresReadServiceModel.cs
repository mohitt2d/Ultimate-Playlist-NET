#region Usings

using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Leaderboard
{
    public class LeaderboardOtherUserScoresReadServiceModel : BaseReadServiceModel
    {
        public string AvatarUrl { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public int Amount { get; set; }
    }
}
