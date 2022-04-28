namespace UltimatePlaylist.Services.Common.Models.Identity
{
    public class EmailChangedConfirmationWriteServiceModel
    {
        public string Token { get; set; }

        public bool IsFromWeb { get; set; }
    }
}
