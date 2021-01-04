using Newtonsoft.Json;
using System;

namespace uRADMonitorX.uRADMonitor.API.V1
{
    public class DeviceDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "timefirst")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? JoinedAt { get; set; }

        [JsonProperty(PropertyName = "timelast")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? LastSeenAt { get; set; }

        [JsonProperty(PropertyName = "timelocal")]
        public long? Uptime { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "altitude")]
        public double? Altitude { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public double? Speed { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "versionsw")]
        public string FirmwareVersion { get; set; }

        [JsonProperty(PropertyName = "versionhw")]
        public string HardwareVersion { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string IsMobile { get; set; }

        [JsonProperty(PropertyName = "placement")]
        public string Placement { get; set; }

        [JsonProperty(PropertyName = "detector")]
        public string DetectorName { get; set; }

        [JsonProperty(PropertyName = "factor")]
        public decimal? DetectorFactor { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "picture")]
        public string Picture { get; set; }

        [JsonProperty(PropertyName = "avg_timelocal")]
        public long? LocalTimeAverage { get; set; }

        [JsonProperty(PropertyName = "avg_temperature")]
        public decimal? TemperatureAverage { get; set; }

        [JsonProperty(PropertyName = "avg_pressure")]
        public decimal? PressureAverage { get; set; }

        [JsonProperty(PropertyName = "avg_humidity")]
        public decimal? HumidityAverage { get; set; }

        [JsonProperty(PropertyName = "avg_voc")]
        public decimal? VocAverage { get; set; }

        [JsonProperty(PropertyName = "min_voc")]
        public decimal? VocMinimum { get; set; }

        [JsonProperty(PropertyName = "max_voc")]
        public decimal? VocMaximum { get; set; }

        [JsonProperty(PropertyName = "avg_noise")]
        public decimal? NoiseAverage { get; set; }

        [JsonProperty(PropertyName = "avg_co2")]
        public decimal? CO2Average { get; set; }

        [JsonProperty(PropertyName = "avg_ch2o")]
        public decimal? CH2OAverage { get; set; }

        [JsonProperty(PropertyName = "avg_o3")]
        public decimal? O3Average { get; set; }

        [JsonProperty(PropertyName = "avg_pm1")]
        public decimal? PM1Average { get; set; }

        [JsonProperty(PropertyName = "avg_pm25")]
        public decimal? PM25Average { get; set; }

        [JsonProperty(PropertyName = "avg_pm10")]
        public decimal? PM10Avereage { get; set; }

        [JsonProperty(PropertyName = "avg_gas1")]
        public decimal? Gas1Average { get; set; }

        [JsonProperty(PropertyName = "avg_gas2")]
        public decimal? Gas2Average { get; set; }

        [JsonProperty(PropertyName = "avg_gas3")]
        public decimal? Gas3Average { get; set; }

        [JsonProperty(PropertyName = "avg_gas4")]
        public decimal? Gas4Average { get; set; }

        [JsonProperty(PropertyName = "avg_cpm")]
        public decimal? CpmAverage { get; set; }

        [JsonProperty(PropertyName = "avg_voltage")]
        public decimal? VoltageAverage { get; set; }

        [JsonProperty(PropertyName = "avg_duty")]
        public decimal? DutyAverage { get; set; }
    }
}
