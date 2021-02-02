using Newtonsoft.Json;
using System.IO;
using System.Linq;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Configuration
{
    public class JsonSettings : Settings
    {
        [JsonIgnore]
        public string FilePath { get; private set; }

        [JsonConstructor]
        private JsonSettings(string filePath) : base()
        {
            this.FilePath = filePath;
        }

        public static void CreateFile(string filePath)
        {
            var jsonSettings = new JsonSettings(filePath);

            jsonSettings.General.StartWithWindows = DefaultSettings.General.StartWithWindows;
            jsonSettings.General.AutomaticallyCheckForUpdates = DefaultSettings.General.AutomaticallyCheckForUpdates;

            jsonSettings.Display.StartMinimized = DefaultSettings.Display.StartMinimized;
            jsonSettings.Display.ShowInTaskbar = DefaultSettings.Display.ShowInTaskbar;
            jsonSettings.Display.CloseToSystemTray = DefaultSettings.Display.CloseToSystemTray;
            jsonSettings.Display.WindowPosition = DefaultSettings.Display.WindowPosition;

            jsonSettings.Notifications.TemperatureThreshold = DefaultSettings.Notifications.TemperatureThreshold;
            jsonSettings.Notifications.RadiationThreshold = DefaultSettings.Notifications.RadiationThreshold;

            jsonSettings.Logging.DirectoryPath = DefaultSettings.Logging.DirectoryPath;
            jsonSettings.Logging.DataLogging.IsEnabled = DefaultSettings.Logging.DataLogging.IsEnabled;
            jsonSettings.Logging.DataLogging.UseSeparateFile = DefaultSettings.Logging.DataLogging.UseSeparateFile;
            jsonSettings.Logging.DataLogging.DirectoryPath = DefaultSettings.Logging.DataLogging.DirectoryPath;

            jsonSettings.Misc.TemperatureUnitType = DefaultSettings.Misc.TemperatureUnitType;
            jsonSettings.Misc.PressureUnitType = DefaultSettings.Misc.PressureUnitType;
            jsonSettings.Misc.RadiationUnitType = DefaultSettings.Misc.RadiationUnitType;

            jsonSettings.IsPollingEnabled = DefaultSettings.Polling.IsEnabled;

            var deviceConfiguration = new DeviceSettings();

            deviceConfiguration.Polling.IsEnabled = DefaultSettings.Polling.IsEnabled;
            deviceConfiguration.Polling.Type = DefaultSettings.Polling.Type;
            deviceConfiguration.Polling.Interval = DefaultSettings.Polling.Interval;

            jsonSettings.Devices.Add(deviceConfiguration);

            jsonSettings.Save();
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

            var deviceSettings = jsonSettings.Devices.FirstOrDefault();

            if (deviceSettings.IsNotNull())
            {
                deviceSettings.Polling.IsEnabled = jsonSettings.IsPollingEnabled;
            }

            jsonSettings.FilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".json");

            return jsonSettings;
        }

        public override void Save()
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
