#region

using System;
using UltimatePlaylist.MobileApi.Models.CommonData;

#endregion

namespace UltimatePlaylist.MobileApi.Models.UserProfile
{
    public class UserProfileInfoResponseModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public UserAvatarFileResponseModel Avatar { get; set; }

        public string ZipCode { get; set; }

        public DateTime? BirthDate { get; set; }

        public GenderResponseModel Gender { get; set; }

        public bool IsConnectedToAppleMusic { get; set; }

        public bool IsConnectedToSpotify { get; set; }

        public bool IsNotificationEnabled { get; set; }

        public bool IsSmsPromotionalNotificationEnabled { get; set; }

        public bool IsPinEnabled { get; set; }

        public bool IsEmailChangingProcessStarted { get; set; }
    }
}
