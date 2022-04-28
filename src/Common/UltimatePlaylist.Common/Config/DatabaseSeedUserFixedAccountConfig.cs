#region Usings

using System;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class DatabaseSeedUserFixedAccountConfig
    {
        public Guid ExternalId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public Guid GenderExternalId { get; set; }

        public string Pin { get; set; }
    }
}
