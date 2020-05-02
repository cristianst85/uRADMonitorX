using uRADMonitorX.Core;

namespace uRADMonitorX.Configuration
{
    public interface ISettings
    {
        // General
        bool StartWithWindows { get; set; }

        bool AutomaticallyCheckForUpdates { get; set; }

        // Display
        bool StartMinimized { get; set; }

        bool ShowInTaskbar { get; set; }

        bool CloseToSystemTray { get; set; }

        int LastWindowXPos { get; set; }

        int LastWindowYPos { get; set; }

        // Logging
        bool IsLoggingEnabled { get; set; }

        string LogDirectoryPath { get; set; }

        bool IsDataLoggingEnabled { get; set; }

        bool DataLoggingToSeparateFile { get; set; }

        string DataLogDirectoryPath { get; set; }

        // Device
        string DetectorName { get; set; }

        bool HasPressureSensor { get; set; }

        string DeviceIPAddress { get; set; }

        TemperatureUnitType TemperatureUnitType { get; set; }

        PressureUnitType PressureUnitType { get; set; }

        RadiationUnitType RadiationUnitType { get; set; }

        PollingType PollingType { get; set; }

        int PollingInterval { get; set; }

        bool IsPollingEnabled { get; set; }

        // Notifications
        bool AreNotificationsEnabled { get; set; }

        int HighTemperatureNotificationValue { get; set; }

        double RadiationNotificationValue { get; set; }

        TemperatureUnitType TemperatureNotificationUnitType { get; set; }

        RadiationUnitType RadiationNotificationUnitType { get; set; }

        // Commit
        void Commit();
    }
}
