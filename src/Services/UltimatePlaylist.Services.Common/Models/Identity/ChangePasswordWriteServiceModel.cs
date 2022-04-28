namespace UltimatePlaylist.Services.Common.Models.Identity
{
    public class ChangePasswordWriteServiceModel
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}