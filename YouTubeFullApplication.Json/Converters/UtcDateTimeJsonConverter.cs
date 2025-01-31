using System.Text.Json;
using System.Text.Json.Serialization;

namespace YouTubeFullApplication.Json.Converters
{
    public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
    {
        private readonly string serializationFormat;

        public UtcDateTimeJsonConverter() : this(null)
        {
        }

        public UtcDateTimeJsonConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-ddTHH:mm:ss.fffffffZ";
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime().ToLocalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            string stringValue = (value.Kind != DateTimeKind.Utc ? value.ToUniversalTime() : value).ToString(serializationFormat);
            writer.WriteStringValue(stringValue);
        }
    }
}
