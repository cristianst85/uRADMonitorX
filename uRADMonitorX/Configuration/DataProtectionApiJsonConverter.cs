using Newtonsoft.Json;
using System;
using uRADMonitorX.Commons.Cryptography;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Configuration
{
    public class DataProtectionApiJsonConverter : JsonConverter
    {
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value.IsNull() || ((string)reader.Value).IsEmpty())
            {
                return reader.Value;
            }

            return DataProtectionApiWrapper.Decrypt((string)reader.Value);
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.IsNull())
            {
                writer.WriteNull();
                return;
            }

            if (((string)value).IsEmpty())
            {
                writer.WriteValue(value);
                return;
            }

            writer.WriteValue(DataProtectionApiWrapper.Encrypt((string)value));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
