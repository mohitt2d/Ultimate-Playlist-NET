namespace UltimatePlaylist.AdminApi.Models.User
{
    public class UserAgeVerificationRequestModel
    {
        public Guid UserExternalId { get; set; }

        public bool IsAgeVerified { get; set; }
    }
}
