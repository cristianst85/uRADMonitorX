using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using uRADMonitorX.Core;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Configuration
{
    public abstract class Settings : ISettings
    {
        public bool IsPollingEnabled { get; set; }

        public GeneralSettings General { get; set; }

        public DisplaySettings Display { get; set; }

        public LoggingSettings Logging { get; set; }

        public NotificationsSettings Notifications { get; set; }

        public MiscSettings Misc { get; set; }

        public IList<DeviceSettings> Devices { get; set; }

        [SuppressMessage(category: "Style", checkId: "IDE1006")]
        public uRADMonitorNetworkSettings uRADMonitorNetwork { get; set; }

        [JsonConstructor]
        public Settings()
        {
            this.General = new GeneralSettings();
            this.Display = new DisplaySettings();
            this.Logging = new LoggingSettings();
            this.Notifications = new NotificationsSettings();
            this.Misc = new MiscSettings();
            this.Devices = new List<DeviceSettings>();
            this.uRADMonitorNetwork = new uRADMonitorNetworkSettings();
        }

        public abstract void Save();
    }

    public sealed class GeneralSettings
    {
        public bool StartWithWindows { get; set; }

        public bool AutomaticallyCheckForUpdates { get; set; }
    }

    public sealed class DisplaySettings
    {
        public bool ShowInTaskbar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is started minimized to System Tray.
        /// </summary>
        public bool StartMinimized { get; set; }

        public bool CloseToSystemTray { get; set; }

        public Point WindowPosition { get; set; }
    }

    public sealed class NotificationsSettings
    {
        public bool IsEnabled { get; set; }

        public NotificationThreshold<TemperatureUnitType> TemperatureThreshold { get; set; }

        public NotificationThreshold<RadiationUnitType> RadiationThreshold { get; set; }

        public NotificationsSettings()
        {
            this.TemperatureThreshold = new NotificationThreshold<TemperatureUnitType>();
            this.RadiationThreshold = new NotificationThreshold<RadiationUnitType>();
        }
    }

    public sealed class LoggingSettings
    {
        public bool IsEnabled { get; set; }

        public string DirectoryPath { get; set; }

        public DataLoggingSettings DataLogging { get; set; }

        public LoggingSettings()
        {
            this.DataLogging = new DataLoggingSettings();
        }
    }

    public sealed class DataLoggingSettings
    {
        public bool IsEnabled { get; set; }

        public bool UseSeparateFile { get; set; }

        public string DirectoryPath { get; set; }
    }

    public sealed class MiscSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TemperatureUnitType TemperatureUnitType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PressureUnitType PressureUnitType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RadiationUnitType RadiationUnitType { get; set; }
    }

    public sealed class DeviceSettings
    {
        public string EndpointUrl { get; set; }

        public DevicePollingSettings Polling { get; set; }

        public IDictionary<string, string> Info { get; private set; }

        [JsonConstructor]
        public DeviceSettings()
        {
            this.Info = new Dictionary<string, string>();
            this.Polling = new DevicePollingSettings();
        }

        public string GetRadiationDetectorName()
        {
            return (string)this.Info.GetValueOrDefault("RadiationDetectorName");
        }

        public void SetRadiationDetectorName(string radiationDetectorName)
        {
            this.Info.AddOrUpdate("RadiationDetectorName", radiationDetectorName);
        }

        public DeviceCapability GetDeviceCapabilities()
        {
            var capabilities = this.Info.GetValueOrDefault("Capabilities");

            return capabilities.IsNotNull() ? (DeviceCapability)Enum.Parse(typeof(DeviceCapability), capabilities) : DeviceCapability.Unknown;
        }

        public void SetDeviceCapabilities(DeviceCapability capabilities)
        {
            this.Info.AddOrUpdate("Capabilities", ((int)capabilities).ToString());
        }
    }

    public sealed class DevicePollingSettings
    {
        public bool IsEnabled { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PollingType Type { get; set; }

        public int? Interval { get; set; }
    }

    [SuppressMessage(category: "Style", checkId: "IDE1006")]
    public sealed class uRADMonitorNetworkSettings
    {
        [JsonConverter(typeof(DataProtectionApiJsonConverter))]
        public string UserId { get; set; }

        [JsonConverter(typeof(DataProtectionApiJsonConverter))]
        public string UserKey { get; set; }
    }
}
