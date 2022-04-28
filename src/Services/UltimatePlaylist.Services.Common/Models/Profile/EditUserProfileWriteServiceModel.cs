#region usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Profile
{
    public class EditUserProfileWriteServiceModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? AvatarExternalId { get; set; }

        public string ZipCode { get; set; }

        public DateTime BirthDate { get; set; }

        public Guid GenderExternalId { get; set; }

        public bool IsNotificationEnabled { get; set; }

        public bool IsSmsPromotionalNotificationEnabled { get; set; }
    }
}
