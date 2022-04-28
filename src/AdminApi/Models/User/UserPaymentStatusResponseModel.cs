#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.AdminApi.Models.User
{
    public class UserPaymentStatusResponseModel
    {
        public Guid UserExternalId { get; set; }

        public WinningStatus PaymentStatus { get; set; }
    }
}
