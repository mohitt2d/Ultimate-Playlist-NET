#region Usings

using System;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Attributes.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Models
{
    public class BaseResponseWithDatesModel : BaseResponseModel
    {
        [FilterColumn(PropertyName.DateCreated)]
        public DateTime Created { get; set; }

        [FilterColumn(PropertyName.DateUpdated)]
        public DateTime? Updated { get; set; }
    }
}
