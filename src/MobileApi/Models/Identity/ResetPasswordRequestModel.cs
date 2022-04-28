namespace UltimatePlaylist.MobileApi.Models.Identity
{
    public class ResetPasswordRequestModel
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}