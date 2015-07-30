using System;
using System.Xml;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Configuration {

    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);

    public class XMLSettings : ISettings {

        public String FilePath { get; private set; }

        // General
        public Boolean StartWithWindows { get; set; }

        // Display
        public Boolean StartMinimized { get; set; }
        public Boolean ShowInTaskbar { get; set; }
        public Boolean CloseToSystemTray { get; set; }
        public int LastWindowXPos { get; set; }
        public int LastWindowYPos { get; set; }

        // Logging
        public Boolean IsLoggingEnabled { get; set; }
        public String LogDirectoryPath { get; set; }

        // Device
        public DeviceInformation Device { get; private set; }
        public bool HasPressureSensor { get; set; }

        public String DeviceIPAddress { get; set; }
        public TemperatureUnitType TemperatureUnitType { get; set; }
        public PressureUnitType PressureUnitType { get; set; }
        public RadiationUnitType RadiationUnitType { get; set; }
        public PollingType PollingType { get; set; }
        public int PollingInterval { get; set; }
        public bool IsPollingEnabled { get; set; }

        public static XmlWriterSettings XmlWriterSettings {
            get {
                return new XmlWriterSettings() {
                    Indent = true,
                    IndentChars = "  "
                };
            }
        }

        private XMLSettings(String filePath) {
            this.FilePath = filePath;
            this.Device = new DeviceInformation();
        }

        /// <summary>
        /// Creates a XMLSettings instance from a XML file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ignoreBadOrMissingValues">If set <c>true</c> ignores bad or missing values and replaces them with default values.</param>
        /// <returns></returns>
        public static XMLSettings LoadFromFile(String filePath, bool ignoreBadOrMissingValues) {
            throw new NotImplementedException();
        }

        public static XMLSettings LoadFromFile(String filePath) {
            XMLSettings xmlSettings = new XMLSettings(filePath);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("/settings");
            xmlSettings.StartWithWindows = bool.Parse(xmlNode["general"].SelectSingleNode("start_with_windows").InnerText);
            xmlSettings.StartMinimized = bool.Parse(xmlNode["display"].SelectSingleNode("start_minimized").InnerText);
            xmlSettings.ShowInTaskbar = bool.Parse(xmlNode["display"].SelectSingleNode("show_in_taskbar").InnerText);
            xmlSettings.CloseToSystemTray = bool.Parse(xmlNode["display"].SelectSingleNode("close_to_system_tray").InnerText);
            xmlSettings.LastWindowXPos = int.Parse(xmlNode["display"].SelectSingleNode("last_window_x_pos").InnerText);
            xmlSettings.LastWindowYPos = int.Parse(xmlNode["display"].SelectSingleNode("last_window_y_pos").InnerText);
            xmlSettings.IsLoggingEnabled = bool.Parse(xmlNode["logging"].SelectSingleNode("enabled").InnerText);
            xmlSettings.LogDirectoryPath = xmlNode["logging"].SelectSingleNode("path").InnerText;
            xmlSettings.HasPressureSensor = bool.Parse(xmlNode["device"].SelectSingleNode("has_pressure_sensor").InnerText);
            xmlSettings.DeviceIPAddress = xmlNode["device"].SelectSingleNode("ip_address").InnerText;
            xmlSettings.TemperatureUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), xmlNode["device"].SelectSingleNode("temperature_unit_type").InnerText, true);
            xmlSettings.PressureUnitType = (PressureUnitType)Enum.Parse(typeof(PressureUnitType), xmlNode["device"].SelectSingleNode("pressure_unit_type").InnerText, true);
            xmlSettings.RadiationUnitType = (RadiationUnitType)Enum.Parse(typeof(RadiationUnitType), xmlNode["device"].SelectSingleNode("radiation_unit_type").InnerText, true);
            xmlSettings.PollingType = (PollingType)Enum.Parse(typeof(PollingType), xmlNode["device"].SelectSingleNode("polling_type").InnerText, true);
            xmlSettings.PollingInterval = int.Parse(xmlNode["device"].SelectSingleNode("polling_interval").InnerText);
            xmlSettings.IsPollingEnabled = bool.Parse(xmlNode["device"].SelectSingleNode("is_polling_enabled").InnerText);
            return xmlSettings;
        }

        public static void CreateFile(string filePath) {
            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings)) {
                xmlWriter.WriteStartElement("settings");
                xmlWriter.WriteStartElement("general");
                writeFullElement(xmlWriter, "start_with_windows", DefaultSettings.StartWithWindows);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("display");
                writeFullElement(xmlWriter, "start_minimized", DefaultSettings.StartMinimized);
                writeFullElement(xmlWriter, "show_in_taskbar", DefaultSettings.ShowInTaskbar);
                writeFullElement(xmlWriter, "close_to_system_tray", DefaultSettings.CloseToSystemTray);
                writeFullElement(xmlWriter, "last_window_x_pos", 50);
                writeFullElement(xmlWriter, "last_window_y_pos", 50);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("logging");
                writeFullElement(xmlWriter, "enabled", DefaultSettings.IsLoggingEnabled);
                writeFullElement(xmlWriter, "path", DefaultSettings.LogDirectoryPath);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("device");
                writeFullElement(xmlWriter, "has_pressure_sensor", DefaultSettings.HasPressureSensor);
                writeFullElement(xmlWriter, "ip_address", DefaultSettings.DeviceIPAddress);
                writeFullElement(xmlWriter, "temperature_unit_type", DefaultSettings.TemperatureUnitType.ToString());
                writeFullElement(xmlWriter, "pressure_unit_type", DefaultSettings.PressureUnitType.ToString());
                writeFullElement(xmlWriter, "radiation_unit_type", DefaultSettings.RadiationUnitType.ToString());
                writeFullElement(xmlWriter, "polling_type", DefaultSettings.PollingType.ToString());
                writeFullElement(xmlWriter, "polling_interval", DefaultSettings.PollingInterval);
                writeFullElement(xmlWriter, "is_polling_enabled", DefaultSettings.IsPollingEnabled);
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        private static void writeFullElement(XmlWriter xmlWriter, String name, object value) {
            xmlWriter.WriteStartElement(name);
            if (value == null) {
                xmlWriter.WriteValue(String.Empty);
            }
            else {
                xmlWriter.WriteValue(value);
            }
            xmlWriter.WriteEndElement();
        }

        public void Commit() {
            this.internalCommit(FilePath);
        }

        private void internalCommit(String filePath) {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("/settings");
            xmlNode["general"].SelectSingleNode("start_with_windows").InnerText = this.StartWithWindows.ToString().ToLower();

            xmlNode["display"].SelectSingleNode("start_minimized").InnerText = this.StartMinimized.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("show_in_taskbar").InnerText = this.ShowInTaskbar.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("close_to_system_tray").InnerText = this.CloseToSystemTray.ToString().ToLower();
            xmlNode["display"].SelectSingleNode("last_window_x_pos").InnerText = this.LastWindowXPos.ToString();
            xmlNode["display"].SelectSingleNode("last_window_y_pos").InnerText = this.LastWindowYPos.ToString();

            xmlNode["logging"].SelectSingleNode("enabled").InnerText = this.IsLoggingEnabled.ToString().ToLower();
            xmlNode["logging"].SelectSingleNode("path").InnerText = this.LogDirectoryPath ?? String.Empty;

            xmlNode["device"].SelectSingleNode("has_pressure_sensor").InnerText = this.HasPressureSensor.ToString().ToLower();
            xmlNode["device"].SelectSingleNode("ip_address").InnerText = this.DeviceIPAddress ?? String.Empty;
            xmlNode["device"].SelectSingleNode("temperature_unit_type").InnerText = this.TemperatureUnitType.ToString();
            xmlNode["device"].SelectSingleNode("pressure_unit_type").InnerText = this.PressureUnitType.ToString();
            xmlNode["device"].SelectSingleNode("radiation_unit_type").InnerText = this.RadiationUnitType.ToString();
            xmlNode["device"].SelectSingleNode("polling_type").InnerText = this.PollingType.ToString();
            xmlNode["device"].SelectSingleNode("polling_interval").InnerText = this.PollingInterval.ToString();
            xmlNode["device"].SelectSingleNode("is_polling_enabled").InnerText = this.IsPollingEnabled.ToString().ToLower();
            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings)) {
                xmlDocument.Save(xmlWriter);
            }
        }
    }
}
