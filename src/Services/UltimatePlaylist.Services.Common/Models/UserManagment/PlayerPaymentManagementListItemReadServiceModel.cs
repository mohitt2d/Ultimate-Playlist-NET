#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.UserManagment
{
    public class PlayerPaymentManagementListItemReadServiceModel
    {
        public string ImageUrl { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public Guid ExternalId { get; set; }

        public DateTime BirthDate { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public WinningStatus PaymentStatus { get; set; }

        public bool AgeVerification { get; set; }

        public GameType WinType { get; set; }

        public DateTime WinningDate { get; set; }

        public Guid WinningExternalId { get; set; }
    }
}
