using Newtonsoft.Json;
using System.IO;

namespace uRADMonitorX.Configuration
{
    public class JsonSettings : Settings
    {
        [JsonIgnore]
        public string FilePath { get; private set; }

        [JsonConstructor]
        private JsonSettings(string filePath)
        {
            this.FilePath = filePath;
        }

        public static void CreateFile(string filePath)
        {
            var jsonSettings = new JsonSettings(filePath)
            {
                StartWithWindows = DefaultSettings.StartWithWindows,
                AutomaticallyCheckForUpdates = DefaultSettings.AutomaticallyCheckForUpdates,

                StartMinimized = DefaultSettings.StartMinimized,
                ShowInTaskbar = DefaultSettings.ShowInTaskbar,
                CloseToSystemTray = DefaultSettings.CloseToSystemTray,
                LastWindowXPos = DefaultSettings.LastWindowXPos,
                LastWindowYPos = DefaultSettings.LastWindowYPos,

                IsLoggingEnabled = DefaultSettings.IsLoggingEnabled,
                LogDirectoryPath = DefaultSettings.LogDirectoryPath,
                IsDataLoggingEnabled = DefaultSettings.IsDataLoggingEnabled,
                DataLoggingToSeparateFile = DefaultSettings.DataLoggingToSeparateFile,
                DataLogDirectoryPath = DefaultSettings.DataLogDirectoryPath,

                DetectorName = DefaultSettings.DetectorName,
                HasPressureSensor = DefaultSettings.HasPressureSensor,
                DeviceIPAddress = DefaultSettings.DeviceIPAddress,
                TemperatureUnitType = DefaultSettings.TemperatureUnitType,
                PressureUnitType = DefaultSettings.PressureUnitType,
                PollingType = DefaultSettings.PollingType,
                PollingInterval = DefaultSettings.PollingInterval,
                IsPollingEnabled = DefaultSettings.IsPollingEnabled,

                AreNotificationsEnabled = DefaultSettings.AreNotificationsEnabled,
                HighTemperatureNotificationValue = DefaultSettings.HighTemperatureNotificationValue,
                RadiationNotificationValue = DefaultSettings.RadiationNotificationValue,
                TemperatureNotificationUnitType = DefaultSettings.TemperatureNotificationUnitType,
                RadiationNotificationUnitType = DefaultSettings.RadiationNotificationUnitType,

                uRADMonitorAPIUserId = null,
                uRADMonitorAPIUserKey = null,
            };

            jsonSettings.Commit();
        }

        public static JsonSettings LoadFromFile(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);

            var jsonSettings = JsonConvert.DeserializeObject<JsonSettings>(fileContent);
            jsonSettings.FilePath = filePath;

            return jsonSettings;
        }

        public static JsonSettings LoadFromXmlFile(string filePath)
        {
            var xmlSettings = XMLSettings.LoadFromFile(filePath);
            var json = JsonConvert.SerializeObject(xmlSettings);

            var jsonSettings = JsonConvert.DeserializeObject<JsonSettings>(json);
            jsonSettings.FilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".json");

            return jsonSettings;
        }

        public override void Commit()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(this.FilePath, json);
        }

        public JsonSettings SaveAs(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(filePath, json);

            return JsonSettings.LoadFromFile(filePath);
        }
    }
}
