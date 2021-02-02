using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using uRADMonitorX.Commons.Cryptography;
using uRADMonitorX.Core;

namespace uRADMonitorX.Configuration
{
    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);

    public class XMLSettings : Settings
    {
        private readonly static NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;

        public string FilePath { get; private set; }

        public DeviceSettings Device { get { return this.Devices.Single(); } }

        public static XmlWriterSettings XmlWriterSettings
        {
            get
            {
                return new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "  "
                };
            }
        }

        private XMLSettings(string filePath) : base()
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Creates a XMLSettings instance from a XML file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ignoreBadOrMissingValues">If set <c>true</c> ignores bad or missing values and replaces them with default values.</param>
        /// <returns></returns>
        public static XMLSettings LoadFromFile(string filePath, bool ignoreBadOrMissingValues)
        {
            throw new NotImplementedException();
        }

        public static XMLSettings LoadFromFile(string filePath)
        {
            var xmlSettings = new XMLSettings(filePath);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);

            var xmlNode = xmlDocument.SelectSingleNode("/settings");
            xmlSettings.General.StartWithWindows = bool.Parse(xmlNode["general"].SelectSingleNode("start_with_windows").InnerText);

            // Introduced with version 1.0.0.
            if (xmlNode["general"].SelectSingleNode("automatically_check_for_updates") != null)
            {
                xmlSettings.General.AutomaticallyCheckForUpdates = bool.Parse(xmlNode["general"].SelectSingleNode("automatically_check_for_updates").InnerText);
            }
            else
            {
                xmlSettings.General.AutomaticallyCheckForUpdates = DefaultSettings.General.AutomaticallyCheckForUpdates;
            }

            xmlSettings.Display.StartMinimized = bool.Parse(xmlNode["display"].SelectSingleNode("start_minimized").InnerText);
            xmlSettings.Display.ShowInTaskbar = bool.Parse(xmlNode["display"].SelectSingleNode("show_in_taskbar").InnerText);
            xmlSettings.Display.CloseToSystemTray = bool.Parse(xmlNode["display"].SelectSingleNode("close_to_system_tray").InnerText);
            xmlSettings.Display.WindowPosition = new Point(int.Parse(xmlNode["display"].SelectSingleNode("last_window_x_pos").InnerText), int.Parse(xmlNode["display"].SelectSingleNode("last_window_y_pos").InnerText));
            xmlSettings.Logging.IsEnabled = bool.Parse(xmlNode["logging"].SelectSingleNode("enabled").InnerText);
            xmlSettings.Logging.DirectoryPath = xmlNode["logging"].SelectSingleNode("path").InnerText;

            // Introduced with version 1.1.0.
            if (xmlNode["logging"].SelectSingleNode("is_data_logging_enabled") != null)
            {
                xmlSettings.Logging.DataLogging.IsEnabled = bool.Parse(xmlNode["logging"].SelectSingleNode("is_data_logging_enabled").InnerText);
            }
            else
            {
                xmlSettings.Logging.DataLogging.IsEnabled = DefaultSettings.Logging.DataLogging.IsEnabled;
            }

            if (xmlNode["logging"].SelectSingleNode("data_logging_to_separate_file") != null)
            {
                xmlSettings.Logging.DataLogging.UseSeparateFile = bool.Parse(xmlNode["logging"].SelectSingleNode("data_logging_to_separate_file").InnerText);
            }
            else
            {
                xmlSettings.Logging.DataLogging.UseSeparateFile = DefaultSettings.Logging.DataLogging.UseSeparateFile;
            }

            if (xmlNode["logging"].SelectSingleNode("data_log_path") != null)
            {
                xmlSettings.Logging.DataLogging.DirectoryPath = xmlNode["logging"].SelectSingleNode("data_log_path").InnerText;
            }
            else
            {
                xmlSettings.Logging.DataLogging.DirectoryPath = DefaultSettings.Logging.DataLogging.DirectoryPath;
            }

            xmlSettings.Devices.Add(new DeviceSettings());

            // Introduced with version 0.39.0.
            if (xmlNode["device"].SelectSingleNode("detector_name") != null)
            {
                xmlSettings.Device.SetRadiationDetectorName(xmlNode["device"].SelectSingleNode("detector_name").InnerText);
            }
            else
            {
                xmlSettings.Device.SetRadiationDetectorName(string.Empty);
            }

            xmlSettings.Device.EndpointUrl = xmlNode["device"].SelectSingleNode("ip_address").InnerText;

            var deviceCapabilities = DeviceCapability.Temperature | DeviceCapability.Radiation;

            if (bool.Parse(xmlNode["device"].SelectSingleNode("has_pressure_sensor").InnerText))
            {
                deviceCapabilities = deviceCapabilities | DeviceCapability.Pressure;
            }

            xmlSettings.Device.SetDeviceCapabilities(deviceCapabilities);

            xmlSettings.Device.Polling.Type = (PollingType)Enum.Parse(typeof(PollingType), xmlNode["device"].SelectSingleNode("polling_type").InnerText, true);
            xmlSettings.Device.Polling.Interval = int.Parse(xmlNode["device"].SelectSingleNode("polling_interval").InnerText);
            xmlSettings.Device.Polling.IsEnabled = bool.Parse(xmlNode["device"].SelectSingleNode("is_polling_enabled").InnerText);

            xmlSettings.IsPollingEnabled = xmlSettings.Device.Polling.IsEnabled;

            xmlSettings.Misc.TemperatureUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), xmlNode["device"].SelectSingleNode("temperature_unit_type").InnerText, true);
            xmlSettings.Misc.PressureUnitType = (PressureUnitType)Enum.Parse(typeof(PressureUnitType), xmlNode["device"].SelectSingleNode("pressure_unit_type").InnerText, true);
            xmlSettings.Misc.RadiationUnitType = (RadiationUnitType)Enum.Parse(typeof(RadiationUnitType), xmlNode["device"].SelectSingleNode("radiation_unit_type").InnerText, true);

            // Notifications were introduced with version 0.39.0.
            // If the 'notifications' element is missing then load the default settings.
            if (xmlNode["notifications"] != null)
            {
                xmlSettings.Notifications.IsEnabled = bool.Parse(xmlNode["notifications"].SelectSingleNode("enabled").InnerText);
                xmlSettings.Notifications.TemperatureThreshold.HighValue = decimal.Parse(xmlNode["notifications"].SelectSingleNode("high_temperature_value").InnerText, NumberStyles.AllowDecimalPoint, numberFormatInfo);
                xmlSettings.Notifications.TemperatureThreshold.MeasurementUnit = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), xmlNode["notifications"].SelectSingleNode("temperature_unit_type").InnerText, true);
                xmlSettings.Notifications.RadiationThreshold.HighValue = decimal.Parse(xmlNode["notifications"].SelectSingleNode("radiation_value").InnerText, NumberStyles.AllowDecimalPoint, numberFormatInfo);
                xmlSettings.Notifications.RadiationThreshold.MeasurementUnit = (RadiationUnitType)Enum.Parse(typeof(RadiationUnitType), xmlNode["notifications"].SelectSingleNode("radiation_unit_type").InnerText, true);
            }
            else
            {
                xmlSettings.Notifications.IsEnabled = DefaultSettings.Notifications.IsEnabled;
                xmlSettings.Notifications.TemperatureThreshold = DefaultSettings.Notifications.TemperatureThreshold;
                xmlSettings.Notifications.RadiationThreshold = DefaultSettings.Notifications.RadiationThreshold;
            }

            // uRADMonitor API credentials were introduced with version 1.4.0.
            if (xmlNode["api_credentials"] != null)
            {
                var userId = xmlNode["api_credentials"].SelectSingleNode("user_id").InnerText;
                xmlSettings.uRADMonitorNetwork.UserId = string.IsNullOrEmpty(userId) ? null : DataProtectionApiWrapper.Decrypt(userId);
                var userKey = xmlNode["api_credentials"].SelectSingleNode("user_key").InnerText;
                xmlSettings.uRADMonitorNetwork.UserKey = string.IsNullOrEmpty(userKey) ? null : DataProtectionApiWrapper.Decrypt(userKey);
            }

            return xmlSettings;
        }

        public static void CreateFile(string filePath)
        {
            using (var xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings))
            {
                xmlWriter.WriteStartElement("settings");
                xmlWriter.WriteStartElement("general");
                WriteFullElement(xmlWriter, "start_with_windows", DefaultSettings.General.StartWithWindows);
                WriteFullElement(xmlWriter, "automatically_check_for_updates", DefaultSettings.General.AutomaticallyCheckForUpdates);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("display");
                WriteFullElement(xmlWriter, "start_minimized", DefaultSettings.Display.StartMinimized);
                WriteFullElement(xmlWriter, "show_in_taskbar", DefaultSettings.Display.ShowInTaskbar);
                WriteFullElement(xmlWriter, "close_to_system_tray", DefaultSettings.Display.CloseToSystemTray);
                WriteFullElement(xmlWriter, "last_window_x_pos", DefaultSettings.Display.WindowPosition.X);
                WriteFullElement(xmlWriter, "last_window_y_pos", DefaultSettings.Display.WindowPosition.Y);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("logging");
                WriteFullElement(xmlWriter, "enabled", DefaultSettings.Logging.IsEnabled);
                WriteFullElement(xmlWriter, "path", DefaultSettings.Logging.DirectoryPath);
                WriteFullElement(xmlWriter, "is_data_logging_enabled", DefaultSettings.Logging.DataLogging.IsEnabled);
                WriteFullElement(xmlWriter, "data_logging_to_separate_file", DefaultSettings.Logging.DataLogging.UseSeparateFile);
                WriteFullElement(xmlWriter, "data_log_path", DefaultSettings.Logging.DataLogging.DirectoryPath);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("device");
                WriteFullElement(xmlWriter, "detector_name", string.Empty);
                WriteFullElement(xmlWriter, "has_pressure_sensor", false);
                WriteFullElement(xmlWriter, "ip_address", string.Empty);
                WriteFullElement(xmlWriter, "temperature_unit_type", DefaultSettings.Misc.TemperatureUnitType.ToString());
                WriteFullElement(xmlWriter, "pressure_unit_type", DefaultSettings.Misc.PressureUnitType.ToString());
                WriteFullElement(xmlWriter, "radiation_unit_type", DefaultSettings.Misc.RadiationUnitType.ToString());
                WriteFullElement(xmlWriter, "polling_type", DefaultSettings.Polling.Type.ToString());
                WriteFullElement(xmlWriter, "polling_interval", DefaultSettings.Polling.Interval);
                WriteFullElement(xmlWriter, "is_polling_enabled", DefaultSettings.Polling.IsEnabled);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("notifications");
                WriteFullElement(xmlWriter, "enabled", DefaultSettings.Notifications.IsEnabled);
                WriteFullElement(xmlWriter, "high_temperature_value", DefaultSettings.Notifications.TemperatureThreshold?.HighValue?.ToString(numberFormatInfo));
                WriteFullElement(xmlWriter, "radiation_value", DefaultSettings.Notifications.RadiationThreshold.HighValue?.ToString(numberFormatInfo));
                WriteFullElement(xmlWriter, "temperature_unit_type", DefaultSettings.Notifications.TemperatureThreshold.MeasurementUnit.ToString());
                WriteFullElement(xmlWriter, "radiation_unit_type", DefaultSettings.Notifications.RadiationThreshold.MeasurementUnit.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("api_credentials");
                WriteFullElement(xmlWriter, "user_id", string.Empty);
                WriteFullElement(xmlWriter, "user_key", string.Empty);
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }
        private static void WriteFullElement(XmlWriter xmlWriter, String name, object value)
        {
            xmlWriter.WriteStartElement(name);

            if (value == null)
            {
                xmlWriter.WriteValue(string.Empty);
            }
            else
            {
                xmlWriter.WriteValue(value);
            }

            xmlWriter.WriteEndElement();
        }

        public override void Save()
        {
            this.InternalCommit(FilePath);
        }

        private void InternalCommit(String filePath)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);

            var xmlNode = xmlDocument.SelectSingleNode("/settings");

            // Introduced with version 1.0.0. 
            if (xmlNode["general"].SelectSingleNode("automatically_check_for_updates") == null)
            {
                xmlNode["general"].InsertAfter(xmlDocument.CreateNode(XmlNodeType.Element, "automatically_check_for_updates", null), xmlNode["general"].SelectSingleNode("start_with_windows"));
            }

            xmlNode["general"].SelectSingleNode("start_with_windows").InnerText = this.General.StartWithWindows.ToString().ToLower();
            xmlNode["general"].SelectSingleNode("automatically_check_for_updates").InnerText = this.General.AutomaticallyCheckForUpdates.ToString().ToLower();

            xmlNode["display"].SelectSingleNode("start_minimized").InnerText = this.Display.StartMinimized.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("show_in_taskbar").InnerText = this.Display.ShowInTaskbar.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("close_to_system_tray").InnerText = this.Display.CloseToSystemTray.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("last_window_x_pos").InnerText = this.Display.WindowPosition.X.ToString();
            xmlNode["display"].SelectSingleNode("last_window_y_pos").InnerText = this.Display.WindowPosition.Y.ToString();

            xmlNode["logging"].SelectSingleNode("enabled").InnerText = this.Logging.IsEnabled.ToString().ToLower();
            xmlNode["logging"].SelectSingleNode("path").InnerText = this.Logging.DirectoryPath ?? string.Empty;

            // Introduced with version 1.1.0.
            CreateNodeIfNotExists(xmlDocument, xmlNode, "logging", "is_data_logging_enabled", "path",
                this.Logging.DataLogging.IsEnabled.ToString().ToLower());
            CreateNodeIfNotExists(xmlDocument, xmlNode, "logging", "data_logging_to_separate_file", "is_data_logging_enabled",
                this.Logging.DataLogging.UseSeparateFile.ToString().ToLower());
            CreateNodeIfNotExists(xmlDocument, xmlNode, "logging", "data_log_path", "data_logging_to_separate_file",
                this.Logging.DataLogging.DirectoryPath ?? string.Empty);

            // Introduced with version 0.39.0. 
            if (xmlNode["device"].SelectSingleNode("detector_name") == null)
            {
                xmlNode["device"].InsertBefore(xmlDocument.CreateNode(XmlNodeType.Element, "detector_name", null), xmlNode["device"].SelectSingleNode("has_pressure_sensor"));
            }

            xmlNode["device"].SelectSingleNode("detector_name").InnerText = this.Device.GetRadiationDetectorName() ?? string.Empty;
            xmlNode["device"].SelectSingleNode("has_pressure_sensor").InnerText = this.Device.GetDeviceCapabilities().HasFlag(DeviceCapability.Pressure).ToString().ToLower();
            xmlNode["device"].SelectSingleNode("ip_address").InnerText = this.Device.EndpointUrl ?? string.Empty;
            xmlNode["device"].SelectSingleNode("temperature_unit_type").InnerText = this.Misc.TemperatureUnitType.ToString();
            xmlNode["device"].SelectSingleNode("pressure_unit_type").InnerText = this.Misc.PressureUnitType.ToString();
            xmlNode["device"].SelectSingleNode("radiation_unit_type").InnerText = this.Misc.RadiationUnitType.ToString();
            xmlNode["device"].SelectSingleNode("polling_type").InnerText = this.Device.Polling.Type.ToString();
            xmlNode["device"].SelectSingleNode("polling_interval").InnerText = this.Device.Polling.Interval.ToString();
            xmlNode["device"].SelectSingleNode("is_polling_enabled").InnerText = this.IsPollingEnabled.ToString().ToLower();

            // Notifications were introduced with version 0.39.0. 
            // If the 'notifications' element is missing then it must be created.
            if (xmlNode["notifications"] == null)
            {
                XmlNode notificationsXmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "notifications", null);
                notificationsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "enabled", null));
                notificationsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "high_temperature_value", null));
                notificationsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "temperature_unit_type", null));
                notificationsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "radiation_value", null));
                notificationsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "radiation_unit_type", null));
                xmlNode.InsertAfter(notificationsXmlNode, xmlNode["device"]);
            }

            xmlNode["notifications"].SelectSingleNode("enabled").InnerText = this.Notifications.IsEnabled.ToString().ToLower();
            xmlNode["notifications"].SelectSingleNode("high_temperature_value").InnerText = this.Notifications.TemperatureThreshold.HighValue.ToString();
            xmlNode["notifications"].SelectSingleNode("temperature_unit_type").InnerText = this.Notifications.TemperatureThreshold.MeasurementUnit.ToString();
            xmlNode["notifications"].SelectSingleNode("radiation_value").InnerText = this.Notifications.RadiationThreshold.HighValue?.ToString(numberFormatInfo);
            xmlNode["notifications"].SelectSingleNode("radiation_unit_type").InnerText = this.Notifications.RadiationThreshold.MeasurementUnit.ToString();

            // uRADMonitor API credentials were introduced with version 1.4.0.
            if (xmlNode["api_credentials"] == null)
            {
                XmlNode apiCredentialsXmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "api_credentials", null);
                apiCredentialsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "user_id", null));
                apiCredentialsXmlNode.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "user_key", null));
                xmlNode.InsertAfter(apiCredentialsXmlNode, xmlNode["notifications"]);
            }

            xmlNode["api_credentials"].SelectSingleNode("user_id").InnerText = string.IsNullOrEmpty(this.uRADMonitorNetwork.UserId) ? null : DataProtectionApiWrapper.Encrypt(this.uRADMonitorNetwork.UserId);
            xmlNode["api_credentials"].SelectSingleNode("user_key").InnerText = string.IsNullOrEmpty(this.uRADMonitorNetwork.UserKey) ? null : DataProtectionApiWrapper.Encrypt(this.uRADMonitorNetwork.UserKey);

            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings))
            {
                xmlDocument.Save(xmlWriter);
            }
        }

        private void CreateNodeIfNotExists(XmlDocument document, XmlNode rootNode, String parentNode, String node, String refNode, String value)
        {
            if (rootNode[parentNode].SelectSingleNode(node) == null)
            {
                rootNode[parentNode].InsertAfter(
                    document.CreateNode(XmlNodeType.Element, node, null),
                    rootNode[parentNode].SelectSingleNode(refNode));
            }

            rootNode[parentNode].SelectSingleNode(node).InnerText = value;
        }
    }
}
