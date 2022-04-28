#region Usings

using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.MobileApi.Models.UserProfile
{
    public class UserAvatarFileResponseModel : BaseResponseModel
    {
        public string Url { get; set; }
    }
}
