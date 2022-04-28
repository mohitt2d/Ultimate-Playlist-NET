#region Usings

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace UltimatePlaylist.Common.Mvc.Converters
{
    public class TicksTimeSpanConverter : JsonConverter<TimeSpan>
    {
        #region Public Methods

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new TimeSpan(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Ticks);
            return;
        }

        #endregion
    }
}
