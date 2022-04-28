#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Models.UserManagment
{
    public class PlayerPaymentManagementFilterReadServiceModel
    {
        public WinningStatus? WinningStatus { get; set; }

        public bool? IsAgeVerified { get; set; }

        public GameType? GameType { get; set; }

        public List<AgeServiceModel> Age { get; set; }
    }
}
