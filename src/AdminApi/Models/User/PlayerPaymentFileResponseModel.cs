#region Usings

using UltimatePlaylist.Common.Mvc.Attributes;

#endregion

namespace UltimatePlaylist.AdminApi.Models.User
{
    public class PlayerPaymentFileResponseModel
    {
        [UltimateColumn("#")]
        public int Number { get; set; }

        [UltimateColumn("Prize Tier")]
        public int Prize { get; set; }

        [UltimateColumn("Entry ID")]
        public int? EntryId { get; set; }

        [UltimateColumn("Player ID")]
        public Guid PlayerId { get; set; }

        [UltimateColumn("First Name")]
        public string FirstName { get; set; }

        [UltimateColumn("Last Name")]
        public string LastName { get; set; }

        [UltimateColumn("Street Address 1")]
        public string StreetAddress1 { get; set; }

        [UltimateColumn("Street Address 2")]
        public string StreetAddress2 { get; set; }

        [UltimateColumn("City")]
        public string City { get; set; }

        [UltimateColumn("State")]
        public string State { get; set; }

        [UltimateColumn("Zip")]
        public string Zip { get; set; }

        [UltimateColumn("Phone")]
        public string Phone { get; set; }

        [UltimateColumn("Email")]
        public string Email { get; set; }

        [UltimateColumn("DOB")]
        public DateTime BirthDate { get; set; }

        [UltimateColumn("Date/Time Stamp")]
        public DateTime DateOfWinning { get; set; }

        [UltimateColumn("Game-Pack-Ticket")]
        public string GamePackTicket { get; set; } = "999999999999999999999";
    }
}
