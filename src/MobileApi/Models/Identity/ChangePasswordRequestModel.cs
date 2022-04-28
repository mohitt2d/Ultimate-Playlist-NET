namespace UltimatePlaylist.MobileApi.Models.Identity
{
    public class ChangePasswordRequestModel
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}