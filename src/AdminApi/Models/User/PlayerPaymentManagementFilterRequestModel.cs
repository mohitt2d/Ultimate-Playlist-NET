#region Usings

using UltimatePlaylist.AdminApi.Models.Song;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.AdminApi.Models.User
{
    public class PlayerPaymentManagementFilterRequestModel
    {
        public WinningStatus? WinningStatus { get; set; }

        public bool? IsAgeVerified { get; set; }

        public GameType? GameType { get; set; }

        public List<AgeRequestModel> Age { get; set; }
    }
}
