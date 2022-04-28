#region Usings

using System;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Attributes.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Models
{
    public class BaseResponseModel
    {
        [FilterColumn(PropertyName.ExternalId)]
        public Guid ExternalId { get; set; }
    }
}
