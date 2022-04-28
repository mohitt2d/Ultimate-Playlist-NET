#region Usings

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UltimatePlaylist.Common.Mvc.Converters;

#endregion

namespace UltimatePlaylist.Common.Mvc.Extensions
{
    public static class NewtonsoftJsonSerializerSettingsExtensions
    {
        public static void SetupJsonSettings(this JsonSerializerSettings jsonSerializerSettings)
        {
            if (jsonSerializerSettings is null)
            {
                throw new ArgumentNullException(nameof(jsonSerializerSettings));
            }

            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
            jsonSerializerSettings.Converters.Add(
                new StringEnumConverter
                {
                    AllowIntegerValues = false,
                });
            jsonSerializerSettings.Converters.Add(new UnixTimestampConverter());
            jsonSerializerSettings.Converters.Add(new TimeSpanConverter());
        }
    }
}
