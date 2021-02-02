using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using uRADMonitorX.Core;

namespace uRADMonitorX.Configuration
{
    public abstract class Settings : ISettings
    {
        // General
        public bool StartWithWindows { get; set; }

        public bool AutomaticallyCheckForUpdates { get; set; }

        // Display
        public bool StartMinimized { get; set; }

        public bool ShowInTaskbar { get; set; }

        public bool CloseToSystemTray { get; set; }

        public int LastWindowXPos { get; set; }

        public int LastWindowYPos { get; set; }

        // Logging
        public bool IsLoggingEnabled { get; set; }

        public string LogDirectoryPath { get; set; }

        public bool IsDataLoggingEnabled { get; set; }

        public bool DataLoggingToSeparateFile { get; set; }

        public string DataLogDirectoryPath { get; set; }

        // Device
        public string DetectorName { get; set; }

        public bool HasPressureSensor { get; set; }

        public string DeviceIPAddress { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemperatureUnitType TemperatureUnitType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PressureUnitType PressureUnitType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RadiationUnitType RadiationUnitType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PollingType PollingType { get; set; }

        public int PollingInterval { get; set; }

        public bool IsPollingEnabled { get; set; }

        // Notifications
        public bool AreNotificationsEnabled { get; set; }

        public int HighTemperatureNotificationValue { get; set; }

        public double RadiationNotificationValue { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemperatureUnitType TemperatureNotificationUnitType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RadiationUnitType RadiationNotificationUnitType { get; set; }

        // uRADMonitor API Authentication
        [JsonConverter(typeof(DataProtectionApiJsonConverter))]
        public string uRADMonitorAPIUserId { get; set; }

        [JsonConverter(typeof(DataProtectionApiJsonConverter))]
        public string uRADMonitorAPIUserKey { get; set; }

        // Commit
        public abstract void Commit();
    }
}
