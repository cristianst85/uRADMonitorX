using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace uRADMonitorX.Configuration
{
    public class NotificationThreshold<T> : IEquatable<NotificationThreshold<T>> where T : struct, IConvertible
    {
        public bool IsEnabled { get; set; }

        public decimal? LowValue { get; set; }

        public decimal? HighValue { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public T MeasurementUnit { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as NotificationThreshold<T>);
        }

        public bool Equals(NotificationThreshold<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return this.IsEnabled == other.IsEnabled &&
                this.LowValue == other.LowValue &&
                this.HighValue == other.HighValue &&
                object.Equals(this.MeasurementUnit, other.MeasurementUnit);
        }
    }
}

