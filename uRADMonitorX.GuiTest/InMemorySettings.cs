using System;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX.GuiTest {

    public class InMemorySettings : ISettings {

        // General
        public Boolean StartWithWindows { get; set; }
        public Boolean AutomaticallyCheckForUpdates { get; set; }

        // Display
        public Boolean StartMinimized { get; set; }
        public Boolean ShowInTaskbar { get; set; }
        public Boolean CloseToSystemTray { get; set; }
        public int LastWindowXPos { get; set; }
        public int LastWindowYPos { get; set; }

        // Logging
        public Boolean IsLoggingEnabled { get; set; }
        public String LogDirectoryPath { get; set; }
        public Boolean IsDataLoggingEnabled { get; set; }
        public Boolean DataLoggingToSeparateFile { get; set; }
        public String DataLogDirectoryPath { get; set; }

        // Device
        public String DetectorName { get; set; }

        public bool HasPressureSensor { get; set; }

        public String DeviceIPAddress { get; set; }
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

        public InMemorySettings() {
        }

        public void Commit() {
            // Nothing to do here.
        }
    }
}
