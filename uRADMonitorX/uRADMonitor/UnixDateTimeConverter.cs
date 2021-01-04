using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace uRADMonitorX.uRADMonitor
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime UnixEpochStartDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return default(DateTime?);
            }

            long seconds = (long)Convert.ChangeType(reader.Value, typeof(long));

            return UnixEpochStartDateTime.AddSeconds(seconds);
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // CanWrite method returns false.
            throw new NotImplementedException();
        }
    }
}
