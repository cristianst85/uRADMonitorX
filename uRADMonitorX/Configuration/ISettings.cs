using System;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Configuration {

    public interface ISettings {

        // General
        Boolean StartWithWindows { get; set; }
        Boolean AutomaticallyCheckForUpdates { get; set; }

        // Display
        Boolean StartMinimized { get; set; }
        Boolean ShowInTaskbar { get; set; }
        Boolean CloseToSystemTray { get; set; }
        int LastWindowXPos { get; set; }
        int LastWindowYPos { get; set; }

        // Logging
        Boolean IsLoggingEnabled { get; set; }
        String LogDirectoryPath { get; set; }
        Boolean IsDataLoggingEnabled { get; set; }
        Boolean DataLoggingToSeparateFile { get; set; }
        String DataLogDirectoryPath { get; set; }

        // Device
        String DetectorName { get; set; }

        bool HasPressureSensor { get; set; }

        String DeviceIPAddress { get; set; }
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

        void Commit();

    }
}
