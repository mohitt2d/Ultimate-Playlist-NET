namespace UltimatePlaylist.AdminApi.Models.User
{
    public class UserAgeVerificationResponseModel
    {
        public Guid UserExternalId { get; set; }

        public bool IsAgeVerified { get; set; }
    }
}
