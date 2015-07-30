using System;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Configuration {

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
        /// Value: <c>false</c>.
        /// </summary>
        public static Boolean IsLoggingEnabled {
            get {
                return false;
            }
        }
        /// <summary>
        /// Value: <c>String.Empty</c>.
        /// </summary>
        public static String LogDirectoryPath {
            get {
                return String.Empty;
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
        /// Value: <c>String.Empty</c>.
        /// </summary>
        public static String DeviceIPAddress {
            get {
                return String.Empty;
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
    }
}
