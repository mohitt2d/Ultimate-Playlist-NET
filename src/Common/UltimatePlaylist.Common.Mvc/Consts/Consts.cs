#region Usings

using System.Collections.Generic;
using Microsoft.Extensions.Logging;

#endregion

namespace UltimatePlaylist.Common.Mvc.Consts
{
    public struct Consts
    {
        #region LoggingEvents

        public struct LoggingEvents
        {
            public static EventId UnhandledException = new EventId(100, "Unhandled exception has occurred.");
        }

        #endregion

        #region Uri

        public struct Uri
        {
            #region Private Members

            private const string GoogleMapsTimezoneApiUrl = "https://maps.googleapis.com/maps/api/timezone/json";
            private const string GoogleMapsLocationApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";

            #endregion

            #region Public Properties

            public static System.Uri GoogleMapsTimezoneApiUri
            {
                get { return new System.Uri(GoogleMapsTimezoneApiUrl); }
            }

            public static System.Uri GoogleMapsLocationApiUri
            {
                get { return new System.Uri(GoogleMapsLocationApiUrl); }
            }

            #endregion
        }

        #endregion

        #region Headers

        public struct XParameters
        {
            public const string PageSize = "x-pageSize";
            public const string PageNumber = "x-pageNumber";
            public const string Query = "x-query";
            public const string Order = "x-order";
            public const string Desc = "x-desc";
        }

        #endregion

        #region MobileApi Groups

        public struct MobileApiGroups
        {
            public const string User = "user";
            public const string CommonData = "commonData";

            public static IDictionary<string, string> GetNameValueDictionary()
            {
                var ret = new Dictionary<string, string>();

                object structValue = default(MobileApiGroups);

                foreach (var group in typeof(MobileApiGroups).GetFields())
                {
                    var value = group.GetValue(structValue).ToString();
                    var name = group.Name;
                    ret.Add(name, value);
                }

                return ret;
            }
        }

        #endregion

        #region AdminApi Groups

        public struct AdminApiGroups
        {
            public const string Administrator = "admin";
            public const string CommonData = "commonData";

            public static IDictionary<string, string> GetNameValueDictionary()
            {
                var ret = new Dictionary<string, string>();

                object structValue = default(AdminApiGroups);

                foreach (var group in typeof(AdminApiGroups).GetFields())
                {
                    var value = group.GetValue(structValue).ToString();
                    var name = group.Name;
                    ret.Add(name, value);
                }

                return ret;
            }
        }

        #endregion

    }
}