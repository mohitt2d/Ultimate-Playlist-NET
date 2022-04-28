namespace UltimatePlaylist.Services.Common.Models.UserManagment
{
    public class PlayerPaymentReadServiceModel
    {
        public int Number { get; set; }

        public decimal Prize { get; set; }

        public Guid PlayerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Zip { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime DateOfWinning { get; set; }
    }
}
