namespace UltimatePlaylist.Services.Common.Models.AppleMusic.Responses
{
    public class AppleMusicPlaylistAttributesResponseModel
    {
        public string Name { get; set; }

        public AppleMusicDescriptionResponseModel Description { get; set; }

        public AppleMusicPlayParamsResponseModel PlayParams { get; set; }

        public bool CanEdit { get; set; }
    }
}
