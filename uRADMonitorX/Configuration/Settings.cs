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

        public TemperatureUnitType TemperatureUnitType { get; set; }

        public PressureUnitType PressureUnitType { get; set; }

        public RadiationUnitType RadiationUnitType { get; set; }

        public PollingType PollingType { get; set; }

        public int PollingInterval { get; set; }

        public bool IsPollingEnabled { get; set; }

        // Notifications
        public bool AreNotificationsEnabled { get; set; }

        public int HighTemperatureNotificationValue { get; set; }

        public double RadiationNotificationValue { get; set; }

        public TemperatureUnitType TemperatureNotificationUnitType { get; set; }

        public RadiationUnitType RadiationNotificationUnitType { get; set; }

        // Commit
        public abstract void Commit();
    }
}
