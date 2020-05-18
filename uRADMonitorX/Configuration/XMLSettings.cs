﻿using System;
using System.Globalization;
using System.Xml;
using uRADMonitorX.Core;

namespace uRADMonitorX.Configuration
{
    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);

    public class XMLSettings : Settings
    {
        private static NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;

        public string FilePath { get; private set; }

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

        private XMLSettings(string filePath)
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
            xmlSettings.StartWithWindows = bool.Parse(xmlNode["general"].SelectSingleNode("start_with_windows").InnerText);

            // Introduced with version 1.0.0.
            if (xmlNode["general"].SelectSingleNode("automatically_check_for_updates") != null)
            {
                xmlSettings.AutomaticallyCheckForUpdates = bool.Parse(xmlNode["general"].SelectSingleNode("automatically_check_for_updates").InnerText);
            }
            else
            {
                xmlSettings.AutomaticallyCheckForUpdates = DefaultSettings.AutomaticallyCheckForUpdates;
            }

            xmlSettings.StartMinimized = bool.Parse(xmlNode["display"].SelectSingleNode("start_minimized").InnerText);
            xmlSettings.ShowInTaskbar = bool.Parse(xmlNode["display"].SelectSingleNode("show_in_taskbar").InnerText);
            xmlSettings.CloseToSystemTray = bool.Parse(xmlNode["display"].SelectSingleNode("close_to_system_tray").InnerText);
            xmlSettings.LastWindowXPos = int.Parse(xmlNode["display"].SelectSingleNode("last_window_x_pos").InnerText);
            xmlSettings.LastWindowYPos = int.Parse(xmlNode["display"].SelectSingleNode("last_window_y_pos").InnerText);
            xmlSettings.IsLoggingEnabled = bool.Parse(xmlNode["logging"].SelectSingleNode("enabled").InnerText);
            xmlSettings.LogDirectoryPath = xmlNode["logging"].SelectSingleNode("path").InnerText;

            // Introduced with version 1.1.0.
            if (xmlNode["logging"].SelectSingleNode("is_data_logging_enabled") != null)
            {
                xmlSettings.IsDataLoggingEnabled = bool.Parse(xmlNode["logging"].SelectSingleNode("is_data_logging_enabled").InnerText);
            }
            else
            {
                xmlSettings.IsDataLoggingEnabled = DefaultSettings.IsDataLoggingEnabled;
            }

            if (xmlNode["logging"].SelectSingleNode("data_logging_to_separate_file") != null)
            {
                xmlSettings.DataLoggingToSeparateFile = bool.Parse(xmlNode["logging"].SelectSingleNode("data_logging_to_separate_file").InnerText);
            }
            else
            {
                xmlSettings.DataLoggingToSeparateFile = DefaultSettings.DataLoggingToSeparateFile;
            }

            if (xmlNode["logging"].SelectSingleNode("data_log_path") != null)
            {
                xmlSettings.DataLogDirectoryPath = xmlNode["logging"].SelectSingleNode("data_log_path").InnerText;
            }
            else
            {
                xmlSettings.DataLogDirectoryPath = DefaultSettings.DataLogDirectoryPath;
            }

            // Introduced with version 0.39.0.
            if (xmlNode["device"].SelectSingleNode("detector_name") != null)
            {
                xmlSettings.DetectorName = xmlNode["device"].SelectSingleNode("detector_name").InnerText;
            }
            else
            {
                xmlSettings.DetectorName = DefaultSettings.DetectorName;
            }

            xmlSettings.HasPressureSensor = bool.Parse(xmlNode["device"].SelectSingleNode("has_pressure_sensor").InnerText);
            xmlSettings.DeviceIPAddress = xmlNode["device"].SelectSingleNode("ip_address").InnerText;
            xmlSettings.TemperatureUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), xmlNode["device"].SelectSingleNode("temperature_unit_type").InnerText, true);
            xmlSettings.PressureUnitType = (PressureUnitType)Enum.Parse(typeof(PressureUnitType), xmlNode["device"].SelectSingleNode("pressure_unit_type").InnerText, true);
            xmlSettings.RadiationUnitType = (RadiationUnitType)Enum.Parse(typeof(RadiationUnitType), xmlNode["device"].SelectSingleNode("radiation_unit_type").InnerText, true);
            xmlSettings.PollingType = (PollingType)Enum.Parse(typeof(PollingType), xmlNode["device"].SelectSingleNode("polling_type").InnerText, true);
            xmlSettings.PollingInterval = int.Parse(xmlNode["device"].SelectSingleNode("polling_interval").InnerText);
            xmlSettings.IsPollingEnabled = bool.Parse(xmlNode["device"].SelectSingleNode("is_polling_enabled").InnerText);

            // Notifications were introduced with version 0.39.0.
            // If the 'notifications' element is missing then load the default settings.
            if (xmlNode["notifications"] != null)
            {
                xmlSettings.AreNotificationsEnabled = bool.Parse(xmlNode["notifications"].SelectSingleNode("enabled").InnerText);
                xmlSettings.HighTemperatureNotificationValue = int.Parse(xmlNode["notifications"].SelectSingleNode("high_temperature_value").InnerText);
                xmlSettings.TemperatureNotificationUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), xmlNode["notifications"].SelectSingleNode("temperature_unit_type").InnerText, true);
                xmlSettings.RadiationNotificationValue = double.Parse(xmlNode["notifications"].SelectSingleNode("radiation_value").InnerText, NumberStyles.AllowDecimalPoint, numberFormatInfo);
                xmlSettings.RadiationNotificationUnitType = (RadiationUnitType)Enum.Parse(typeof(RadiationUnitType), xmlNode["notifications"].SelectSingleNode("radiation_unit_type").InnerText, true);
            }
            else
            {
                xmlSettings.AreNotificationsEnabled = DefaultSettings.AreNotificationsEnabled;
                xmlSettings.HighTemperatureNotificationValue = DefaultSettings.HighTemperatureNotificationValue;
                xmlSettings.TemperatureNotificationUnitType = DefaultSettings.TemperatureNotificationUnitType;
                xmlSettings.RadiationNotificationValue = DefaultSettings.RadiationNotificationValue;
                xmlSettings.RadiationNotificationUnitType = DefaultSettings.RadiationNotificationUnitType;
            }

            return xmlSettings;
        }

        public static void CreateFile(string filePath)
        {
            using (var xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings))
            {
                xmlWriter.WriteStartElement("settings");
                xmlWriter.WriteStartElement("general");
                writeFullElement(xmlWriter, "start_with_windows", DefaultSettings.StartWithWindows);
                writeFullElement(xmlWriter, "automatically_check_for_updates", DefaultSettings.AutomaticallyCheckForUpdates);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("display");
                writeFullElement(xmlWriter, "start_minimized", DefaultSettings.StartMinimized);
                writeFullElement(xmlWriter, "show_in_taskbar", DefaultSettings.ShowInTaskbar);
                writeFullElement(xmlWriter, "close_to_system_tray", DefaultSettings.CloseToSystemTray);
                writeFullElement(xmlWriter, "last_window_x_pos", DefaultSettings.LastWindowXPos);
                writeFullElement(xmlWriter, "last_window_y_pos", DefaultSettings.LastWindowYPos);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("logging");
                writeFullElement(xmlWriter, "enabled", DefaultSettings.IsLoggingEnabled);
                writeFullElement(xmlWriter, "path", DefaultSettings.LogDirectoryPath);
                writeFullElement(xmlWriter, "is_data_logging_enabled", DefaultSettings.IsDataLoggingEnabled);
                writeFullElement(xmlWriter, "data_logging_to_separate_file", DefaultSettings.DataLoggingToSeparateFile);
                writeFullElement(xmlWriter, "data_log_path", DefaultSettings.DataLogDirectoryPath);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("device");
                writeFullElement(xmlWriter, "detector_name", DefaultSettings.DetectorName);
                writeFullElement(xmlWriter, "has_pressure_sensor", DefaultSettings.HasPressureSensor);
                writeFullElement(xmlWriter, "ip_address", DefaultSettings.DeviceIPAddress);
                writeFullElement(xmlWriter, "temperature_unit_type", DefaultSettings.TemperatureUnitType.ToString());
                writeFullElement(xmlWriter, "pressure_unit_type", DefaultSettings.PressureUnitType.ToString());
                writeFullElement(xmlWriter, "radiation_unit_type", DefaultSettings.RadiationUnitType.ToString());
                writeFullElement(xmlWriter, "polling_type", DefaultSettings.PollingType.ToString());
                writeFullElement(xmlWriter, "polling_interval", DefaultSettings.PollingInterval);
                writeFullElement(xmlWriter, "is_polling_enabled", DefaultSettings.IsPollingEnabled);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("notifications");
                writeFullElement(xmlWriter, "enabled", DefaultSettings.AreNotificationsEnabled);
                writeFullElement(xmlWriter, "high_temperature_value", DefaultSettings.HighTemperatureNotificationValue);
                writeFullElement(xmlWriter, "radiation_value", DefaultSettings.RadiationNotificationValue.ToString(numberFormatInfo));
                writeFullElement(xmlWriter, "temperature_unit_type", DefaultSettings.TemperatureNotificationUnitType.ToString());
                writeFullElement(xmlWriter, "radiation_unit_type", DefaultSettings.RadiationNotificationUnitType.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        private static void writeFullElement(XmlWriter xmlWriter, String name, object value)
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

        public override void Commit()
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

            xmlNode["general"].SelectSingleNode("start_with_windows").InnerText = this.StartWithWindows.ToString().ToLower();
            xmlNode["general"].SelectSingleNode("automatically_check_for_updates").InnerText = this.AutomaticallyCheckForUpdates.ToString().ToLower();

            xmlNode["display"].SelectSingleNode("start_minimized").InnerText = this.StartMinimized.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("show_in_taskbar").InnerText = this.ShowInTaskbar.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("close_to_system_tray").InnerText = this.CloseToSystemTray.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("last_window_x_pos").InnerText = this.LastWindowXPos.ToString();
            xmlNode["display"].SelectSingleNode("last_window_y_pos").InnerText = this.LastWindowYPos.ToString();

            xmlNode["logging"].SelectSingleNode("enabled").InnerText = this.IsLoggingEnabled.ToString().ToLower();
            xmlNode["logging"].SelectSingleNode("path").InnerText = this.LogDirectoryPath ?? string.Empty;

            // Introduced with version 1.1.0.
            CreateNodeIfNotExists(xmlDocument, xmlNode, "logging", "is_data_logging_enabled", "path",
                this.IsDataLoggingEnabled.ToString().ToLower());
            CreateNodeIfNotExists(xmlDocument, xmlNode, "logging", "data_logging_to_separate_file", "is_data_logging_enabled",
                this.DataLoggingToSeparateFile.ToString().ToLower());
            CreateNodeIfNotExists(xmlDocument, xmlNode, "logging", "data_log_path", "data_logging_to_separate_file",
                this.DataLogDirectoryPath ?? string.Empty);

            // Introduced with version 0.39.0. 
            if (xmlNode["device"].SelectSingleNode("detector_name") == null)
            {
                xmlNode["device"].InsertBefore(xmlDocument.CreateNode(XmlNodeType.Element, "detector_name", null), xmlNode["device"].SelectSingleNode("has_pressure_sensor"));
            }

            xmlNode["device"].SelectSingleNode("detector_name").InnerText = this.DetectorName ?? string.Empty;
            xmlNode["device"].SelectSingleNode("has_pressure_sensor").InnerText = this.HasPressureSensor.ToString().ToLower();
            xmlNode["device"].SelectSingleNode("ip_address").InnerText = this.DeviceIPAddress ?? string.Empty;
            xmlNode["device"].SelectSingleNode("temperature_unit_type").InnerText = this.TemperatureUnitType.ToString();
            xmlNode["device"].SelectSingleNode("pressure_unit_type").InnerText = this.PressureUnitType.ToString();
            xmlNode["device"].SelectSingleNode("radiation_unit_type").InnerText = this.RadiationUnitType.ToString();
            xmlNode["device"].SelectSingleNode("polling_type").InnerText = this.PollingType.ToString();
            xmlNode["device"].SelectSingleNode("polling_interval").InnerText = this.PollingInterval.ToString();
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

            xmlNode["notifications"].SelectSingleNode("enabled").InnerText = this.AreNotificationsEnabled.ToString().ToLower();
            xmlNode["notifications"].SelectSingleNode("high_temperature_value").InnerText = this.HighTemperatureNotificationValue.ToString();
            xmlNode["notifications"].SelectSingleNode("temperature_unit_type").InnerText = this.TemperatureNotificationUnitType.ToString();
            xmlNode["notifications"].SelectSingleNode("radiation_value").InnerText = this.RadiationNotificationValue.ToString();
            xmlNode["notifications"].SelectSingleNode("radiation_unit_type").InnerText = this.RadiationNotificationUnitType.ToString();

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
