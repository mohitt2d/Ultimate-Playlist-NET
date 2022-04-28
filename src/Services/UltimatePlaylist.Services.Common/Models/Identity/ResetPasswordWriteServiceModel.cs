namespace UltimatePlaylist.Services.Common.Models.Identity
{
    public class ResetPasswordWriteServiceModel
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}