namespace UltimatePlaylist.AdminApi.Models.User
{
    public class UserStatusRequestModel
    {
        public Guid UserExternalId { get; set; }

        public bool IsActive { get; set; }
    }
}
