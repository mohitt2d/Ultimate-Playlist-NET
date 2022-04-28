#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Analytics
{
    public class SaveAnalyticsDataRequestModel
    {
        public string EventType { get; set; }

        public Dictionary<string, string> Params { get; set; }
    }
}
