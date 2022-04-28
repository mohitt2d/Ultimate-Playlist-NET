#region Usings

using System;
using System.Globalization;
using Newtonsoft.Json;

#endregion

namespace UltimatePlaylist.Common.Mvc.Converters
{
    public class TimeSpanConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan) == objectType || typeof(TimeSpan?) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;

            if (value is null)
            {
                if (Nullable.GetUnderlyingType(objectType) is null)
                {
                    throw new JsonSerializationException("Null value is not allowed.");
                }

                return default;
            }

            if (!(value is double totalSeconds))
            {
                if (!double.TryParse(value.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out totalSeconds))
                {
                    throw new JsonSerializationException($"Unable to parse {value} to double.");
                }
            }

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TimeSpan timeSpan)
            {
                writer.WriteValue(timeSpan.TotalSeconds);
            }
        }

        #endregion
    }
}
