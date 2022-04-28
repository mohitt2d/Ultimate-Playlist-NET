#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Models
{
    public class BaseReadServiceModel
    {
        public Guid ExternalId { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
