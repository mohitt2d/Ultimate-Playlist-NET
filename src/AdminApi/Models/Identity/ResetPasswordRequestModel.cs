namespace UltimatePlaylist.AdminApi.Models.Identity
{
    public class ResetPasswordRequestModel
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}