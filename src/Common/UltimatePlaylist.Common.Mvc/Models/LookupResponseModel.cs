#region Usings

using System;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Attributes.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Models
{
    public class LookupResponseModel
    {
        [FilterColumn(PropertyName.ExternalId)]
        public Guid ExternalId { get; set; }

        [FilterColumn(PropertyName.Name)]
        public string Name { get; set; }

        [FilterColumn(PropertyName.AdditionalInfo)]
        public string AdditionalInfo { get; set; }
    }
}
