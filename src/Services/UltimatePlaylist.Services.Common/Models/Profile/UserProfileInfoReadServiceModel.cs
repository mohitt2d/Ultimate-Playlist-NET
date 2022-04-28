#region Usings

using System;
using UltimatePlaylist.Services.Common.Models.CommonData;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Profile
{
    public class UserProfileInfoReadServiceModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public FileReadServiceModel Avatar { get; set; }

        public string ZipCode { get; set; }

        public DateTime? BirthDate { get; set; }

        public GenderReadServiceModel Gender { get; set; }

        public bool IsConnectedToAppleMusic { get; set; }

        public bool IsConnectedToSpotify { get; set; }

        public bool IsNotificationEnabled { get; set; }

        public bool IsSmsPromotionalNotificationEnabled { get; set; }

        public bool IsPinEnabled { get; set; }

        public bool IsEmailChangingProcessStarted { get; set; }
    }
}
