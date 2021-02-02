using System.Drawing;
using uRADMonitorX.Core;

namespace uRADMonitorX.Configuration
{
    public static class DefaultSettings
    {
        public static GeneralSettings General
        {
            get
            {
                return new GeneralSettings()
                {
                    StartWithWindows = true
                };
            }
        }

        public static DisplaySettings Display
        {
            get
            {
                return new DisplaySettings()
                {
                    StartMinimized = true,
                    CloseToSystemTray = true,
                    WindowPosition = new Point(535, 300)
                };
            }
        }

        public static LoggingSettings Logging
        {
            get
            {
                return new LoggingSettings()
                {
                    DirectoryPath = string.Empty,
                    DataLogging = DataLogging
                };
            }
        }

        private static DataLoggingSettings DataLogging
        {
            get
            {
                return new DataLoggingSettings()
                {
                    UseSeparateFile = true,
                    DirectoryPath = @"\logs"
                };
            }
        }

        public static NotificationsSettings Notifications
        {
            get
            {
                return new NotificationsSettings()
                {
                    TemperatureThreshold = new NotificationThreshold<TemperatureUnitType>() { HighValue = 50.0m, MeasurementUnit = TemperatureUnitType.Celsius },
                    RadiationThreshold = new NotificationThreshold<RadiationUnitType>() { HighValue = 0.5m, MeasurementUnit = RadiationUnitType.uSvH }
                };
            }
        }

        public static MiscSettings Misc
        {
            get
            {
                return new MiscSettings()
                {
                    TemperatureUnitType = TemperatureUnitType.Celsius,
                    PressureUnitType = PressureUnitType.Pa,
                    RadiationUnitType = RadiationUnitType.Cpm
                };
            }
        }

        public static DevicePollingSettings Polling
        {
            get
            {
                return new DevicePollingSettings()
                {
                    IsEnabled = true,
                    Type = PollingType.FixedInterval,
                    Interval = 5
                };
            }
        }
    }
}
