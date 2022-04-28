namespace UltimatePlaylist.Services.Common.Models.Email
{
    public class RegistrationConfirmationRequest
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Token { get; set; }
    }
}