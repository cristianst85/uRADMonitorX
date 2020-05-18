using System;
using uRADMonitorX.Core;

namespace uRADMonitorX.Configuration
{
    public static class DefaultSettings {

        /// <summary>
        /// Value: <c>true</c>.
        /// </summary>
        public static Boolean StartWithWindows {
            get {
                return true;
            }
        }

        /// <summary>
        /// Value: <c>false</c>.
        /// </summary>
        public static Boolean AutomaticallyCheckForUpdates {
            get {
                return false;
            }
        }

        /// <summary>
        /// Value: <c>true</c>.
        /// </summary>
        public static Boolean StartMinimized {
            get {
                return true;
            }
        }

        /// <summary>
        /// Value: <c>false</c>.
        /// </summary>
        public static Boolean ShowInTaskbar {
            get {
                return false;
            }
        }

        /// <summary>
        /// Value: <c>true</c>.
        /// </summary>
        public static Boolean CloseToSystemTray {
            get {
                return true;
            }
        }

        /// <summary>
        /// Value: <c>50</c>.
        /// </summary>
        public static int LastWindowXPos {
            get {
                return 50;
            }
        }

        /// <summary>
        /// Value: <c>50</c>.
        /// </summary>
        public static int LastWindowYPos {
            get {
                return 50;
            }
        }

        /// <summary>
        /// Value: <c>false</c>.
        /// </summary>
        public static Boolean IsLoggingEnabled {
            get {
                return false;
            }
        }

        /// <summary>
        /// Value: <c>string.Empty</c>.
        /// </summary>
        public static string LogDirectoryPath {
            get {
                return string.Empty;
            }
        }

        /// <summary>
        /// Value: <c>false</c>.
        /// </summary>
        public static Boolean IsDataLoggingEnabled {
            get {
                return false;
            }
        }

        /// <summary>
        /// Value: <c>true</c>.
        /// </summary>
        public static Boolean DataLoggingToSeparateFile {
            get {
                return true;
            }
        }

        /// <summary>
        /// Value: <c>\logs</c>.
        /// </summary>
        public static String DataLogDirectoryPath {
            get {
                return @"\logs";
            }
        }

        /// <summary>
        /// Value: <c>string.Empty</c>.
        /// </summary>
        public static String DetectorName {
            get {
                return string.Empty;
            }
        }

        /// <summary>
        /// Value: <c>false</c>.
        /// </summary>
        public static Boolean HasPressureSensor {
            get {
                return false;
            }
        }

        /// <summary>
        /// Value: <c>string.Empty</c>.
        /// </summary>
        public static string DeviceIPAddress {
            get {
                return string.Empty;
            }
        }

        /// <summary>
        /// Value: <c>TemperatureUnitType.Celsius</c>.
        /// </summary>
        public static TemperatureUnitType TemperatureUnitType {
            get {
                return TemperatureUnitType.Celsius;
            }
        }

        /// <summary>
        /// Value: <c>PressureUnitType.Pa</c>.
        /// </summary>
        public static PressureUnitType PressureUnitType {
            get {
                return PressureUnitType.Pa;
            }
        }

        /// <summary>
        /// Value: <c>RadiationUnitType.Cpm</c>.
        /// </summary>
        public static RadiationUnitType RadiationUnitType {
            get {
                return RadiationUnitType.Cpm;
            }
        }

        /// <summary>
        /// Value: <c>PollingType.FixedInterval</c>.
        /// </summary>
        public static PollingType PollingType {
            get {
                return PollingType.FixedInterval;
            }
        }

        /// <summary>
        /// Value: <c>5</c> seconds.
        /// </summary>
        public static int PollingInterval {
            get {
                return 5;
            }
        }

        /// <summary>
        /// Value: <c>true</c>.
        /// </summary>
        public static bool IsPollingEnabled {
            get {
                return true;
            }
        }

        /// <summary>
        /// Value: <c>false</c>.
        /// </summary>
        public static bool AreNotificationsEnabled {
            get {
                return false;
            }
        }

        /// <summary>
        /// Value: <c>50</c> degrees Celsius.
        /// </summary>
        public static int HighTemperatureNotificationValue {
            get {
                return 50;
            }
        }

        /// <summary>
        /// Value: <c>0.5</c> uSvH.
        /// </summary>
        public static double RadiationNotificationValue {
            get {
                return 0.5;
            }
        }

        /// <summary>
        /// Value: <c>TemperatureUnitType.Celsius</c>.
        /// </summary>
        public static TemperatureUnitType TemperatureNotificationUnitType {
            get {
                return TemperatureUnitType.Celsius;
            }
        }

        /// <summary>
        /// Value: <c>RadiationUnitType.uSvH</c>.
        /// </summary>
        public static RadiationUnitType RadiationNotificationUnitType {
            get {
                return RadiationUnitType.uSvH;
            }
        }
    }
}
