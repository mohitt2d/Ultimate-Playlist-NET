#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.AdminApi.Models.User
{
    public class UserPaymentStatusRequestModel
    {
        public Guid WinningExternalId { get; set; }

        public Guid UserExternalId { get; set; }

        public WinningStatus PaymentStatus { get; set; }
    }
}
