using NUnit.Framework;
using System.IO;
using System.Linq;
using uRADMonitorX.Configuration;

namespace uRADMonitorX.Tests.Configuration
{
    [TestFixture]
    public class JsonSettingsTests
    {
        private readonly string xmlSettingsFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\uRADMonitorX.Tests.Files\configs\Settings.xml"));
        private readonly string jsonSettingsFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\uRADMonitorX.Tests.Files\configs\Settings.json"));
        private readonly string jsonSettingsNewFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\uRADMonitorX.Tests.Files\configs\Settings1.json"));

        [SetUp]
        public void SetUp()
        {
            Assert.IsFalse(File.Exists(xmlSettingsFilePath));
            Assert.IsFalse(File.Exists(jsonSettingsFilePath));
            Assert.IsFalse(File.Exists(jsonSettingsNewFilePath));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(xmlSettingsFilePath);
            File.Delete(jsonSettingsFilePath);
            File.Delete(jsonSettingsNewFilePath);

            Assert.IsFalse(File.Exists(xmlSettingsFilePath));
            Assert.IsFalse(File.Exists(jsonSettingsFilePath));
            Assert.IsFalse(File.Exists(jsonSettingsNewFilePath));
        }

        [Test]
        public void LoadFromXmlFile()
        {
            XMLSettings.CreateFile(xmlSettingsFilePath);

            Assert.IsTrue(File.Exists(xmlSettingsFilePath));

            var jsonSettings = JsonSettings.LoadFromXmlFile(xmlSettingsFilePath);
            var xmlSettings = XMLSettings.LoadFromFile(xmlSettingsFilePath);

            Assert.AreEqual(xmlSettingsFilePath, xmlSettings.FilePath);
            Assert.AreNotEqual(xmlSettings.FilePath, jsonSettings.FilePath);

            Assert.AreEqual(".json", Path.GetExtension(jsonSettings.FilePath));

            Assert.AreEqual(xmlSettings.IsPollingEnabled, jsonSettings.IsPollingEnabled);

            Assert.AreEqual(xmlSettings.General.StartWithWindows, jsonSettings.General.StartWithWindows);
            Assert.AreEqual(xmlSettings.General.AutomaticallyCheckForUpdates, jsonSettings.General.AutomaticallyCheckForUpdates);

            Assert.AreEqual(xmlSettings.Display.StartMinimized, jsonSettings.Display.StartMinimized);
            Assert.AreEqual(xmlSettings.Display.ShowInTaskbar, jsonSettings.Display.ShowInTaskbar);
            Assert.AreEqual(xmlSettings.Display.CloseToSystemTray, jsonSettings.Display.CloseToSystemTray);
            Assert.AreEqual(xmlSettings.Display.WindowPosition, jsonSettings.Display.WindowPosition);

            Assert.AreEqual(xmlSettings.Logging.IsEnabled, jsonSettings.Logging.IsEnabled);
            Assert.AreEqual(xmlSettings.Logging.DirectoryPath, jsonSettings.Logging.DirectoryPath);
            Assert.AreEqual(xmlSettings.Logging.DataLogging.IsEnabled, jsonSettings.Logging.DataLogging.IsEnabled);
            Assert.AreEqual(xmlSettings.Logging.DataLogging.UseSeparateFile, jsonSettings.Logging.DataLogging.UseSeparateFile);
            Assert.AreEqual(xmlSettings.Logging.DataLogging.DirectoryPath, jsonSettings.Logging.DataLogging.DirectoryPath);

            Assert.AreEqual(xmlSettings.Device.EndpointUrl, jsonSettings.Devices.Single().EndpointUrl);
            Assert.AreEqual(xmlSettings.Device.GetDeviceCapabilities().HasFlag(DeviceCapability.Pressure), jsonSettings.Devices.Single().GetDeviceCapabilities().HasFlag(DeviceCapability.Pressure));
            Assert.AreEqual(xmlSettings.Device.GetRadiationDetectorName(), jsonSettings.Devices.Single().GetRadiationDetectorName());

            Assert.AreEqual(xmlSettings.Device.Polling.IsEnabled, jsonSettings.Devices.Single().Polling.IsEnabled);
            Assert.AreEqual(xmlSettings.Device.Polling.Type, jsonSettings.Devices.Single().Polling.Type);
            Assert.AreEqual(xmlSettings.Device.Polling.Interval, jsonSettings.Devices.Single().Polling.Interval);

            Assert.AreEqual(xmlSettings.Misc.TemperatureUnitType, jsonSettings.Misc.TemperatureUnitType);
            Assert.AreEqual(xmlSettings.Misc.PressureUnitType, jsonSettings.Misc.PressureUnitType);
            Assert.AreEqual(xmlSettings.Misc.RadiationUnitType, jsonSettings.Misc.RadiationUnitType);

            Assert.AreEqual(xmlSettings.Notifications.IsEnabled, jsonSettings.Notifications.IsEnabled);
            Assert.AreEqual(xmlSettings.Notifications.TemperatureThreshold, jsonSettings.Notifications.TemperatureThreshold);
            Assert.AreEqual(xmlSettings.Notifications.RadiationThreshold, jsonSettings.Notifications.RadiationThreshold);

            Assert.AreEqual(xmlSettings.uRADMonitorNetwork.UserId, jsonSettings.uRADMonitorNetwork.UserId);
            Assert.AreEqual(xmlSettings.uRADMonitorNetwork.UserKey, jsonSettings.uRADMonitorNetwork.UserKey);
        }

        [Test]
        public void LoadFromXmlFile_Enum_SerializationDeserialization()
        {
            XMLSettings.CreateFile(xmlSettingsFilePath);

            Assert.IsTrue(File.Exists(xmlSettingsFilePath));

            var xmlSettings = XMLSettings.LoadFromFile(xmlSettingsFilePath);

            Assert.AreEqual(xmlSettingsFilePath, xmlSettings.FilePath);

            xmlSettings.Notifications.TemperatureThreshold.MeasurementUnit = uRADMonitorX.Core.TemperatureUnitType.Fahrenheit;
            xmlSettings.Notifications.RadiationThreshold.MeasurementUnit = uRADMonitorX.Core.RadiationUnitType.uSvH;

            xmlSettings.Save();

            var jsonSettings = JsonSettings.LoadFromXmlFile(xmlSettingsFilePath);

            Assert.AreNotEqual(xmlSettings.FilePath, jsonSettings.FilePath);
            Assert.AreEqual(".json", Path.GetExtension(jsonSettings.FilePath));

            Assert.AreEqual(xmlSettings.Notifications.TemperatureThreshold.MeasurementUnit, jsonSettings.Notifications.TemperatureThreshold.MeasurementUnit);
            Assert.AreEqual(xmlSettings.Notifications.RadiationThreshold.MeasurementUnit, jsonSettings.Notifications.RadiationThreshold.MeasurementUnit);
        }

        [Test]
        public void CreateFileWithDefaults()
        {
            JsonSettings.CreateFile(jsonSettingsFilePath);

            Assert.IsTrue(File.Exists(jsonSettingsFilePath));

            var settings = JsonSettings.LoadFromFile(jsonSettingsFilePath);

            Assert.AreEqual(jsonSettingsFilePath, settings.FilePath);

            Assert.AreEqual(DefaultSettings.Polling.IsEnabled, settings.IsPollingEnabled);

            Assert.AreEqual(DefaultSettings.General.StartWithWindows, settings.General.StartWithWindows);
            Assert.AreEqual(DefaultSettings.General.AutomaticallyCheckForUpdates, settings.General.AutomaticallyCheckForUpdates);

            Assert.AreEqual(DefaultSettings.Display.StartMinimized, settings.Display.StartMinimized);
            Assert.AreEqual(DefaultSettings.Display.ShowInTaskbar, settings.Display.ShowInTaskbar);
            Assert.AreEqual(DefaultSettings.Display.CloseToSystemTray, settings.Display.CloseToSystemTray);

            Assert.AreEqual(DefaultSettings.Display.WindowPosition, settings.Display.WindowPosition);

            Assert.AreEqual(DefaultSettings.Notifications.IsEnabled, settings.Notifications.IsEnabled);
            Assert.AreEqual(DefaultSettings.Notifications.TemperatureThreshold, settings.Notifications.TemperatureThreshold);
            Assert.AreEqual(DefaultSettings.Notifications.RadiationThreshold, settings.Notifications.RadiationThreshold);

            Assert.AreEqual(DefaultSettings.Logging.IsEnabled, settings.Logging.IsEnabled);
            Assert.AreEqual(DefaultSettings.Logging.DirectoryPath, settings.Logging.DirectoryPath);
            Assert.AreEqual(DefaultSettings.Logging.DataLogging.IsEnabled, settings.Logging.DataLogging.IsEnabled);
            Assert.AreEqual(DefaultSettings.Logging.DataLogging.UseSeparateFile, settings.Logging.DataLogging.UseSeparateFile);
            Assert.AreEqual(DefaultSettings.Logging.DataLogging.DirectoryPath, settings.Logging.DataLogging.DirectoryPath);

            Assert.AreEqual(DefaultSettings.Misc.TemperatureUnitType, settings.Misc.TemperatureUnitType);
            Assert.AreEqual(DefaultSettings.Misc.PressureUnitType, settings.Misc.PressureUnitType);
            Assert.AreEqual(DefaultSettings.Misc.RadiationUnitType, settings.Misc.RadiationUnitType);

            Assert.AreEqual(DefaultSettings.Polling.IsEnabled, settings.Devices.Single().Polling.IsEnabled);
            Assert.AreEqual(DefaultSettings.Polling.Type, settings.Devices.Single().Polling.Type);
            Assert.AreEqual(DefaultSettings.Polling.Interval, settings.Devices.Single().Polling.Interval);

            Assert.IsNull(settings.Devices.Single().GetRadiationDetectorName());
            Assert.IsNull(settings.Devices.Single().EndpointUrl);

            Assert.AreEqual(DeviceCapability.Unknown, settings.Devices.Single().GetDeviceCapabilities());

            Assert.IsNull(settings.uRADMonitorNetwork.UserId);
            Assert.IsNull(settings.uRADMonitorNetwork.UserKey);
        }

        [Test]
        public void Save()
        {
            JsonSettings.CreateFile(jsonSettingsFilePath);

            var jsonSettings = JsonSettings.LoadFromFile(jsonSettingsFilePath);

            Assert.IsFalse(JsonSettings.LoadFromFile(jsonSettingsFilePath).Logging.IsEnabled);

            jsonSettings.Logging.IsEnabled = true;

            jsonSettings.Save();

            Assert.IsTrue(JsonSettings.LoadFromFile(jsonSettingsFilePath).Logging.IsEnabled);
        }

        [Test]
        public void SaveAs()
        {
            JsonSettings.CreateFile(jsonSettingsFilePath);

            var jsonSettings = JsonSettings.LoadFromFile(jsonSettingsFilePath);

            Assert.AreEqual(jsonSettingsFilePath, jsonSettings.FilePath);

            Assert.AreEqual(jsonSettingsNewFilePath, jsonSettings.SaveAs(jsonSettingsNewFilePath).FilePath);

            Assert.IsTrue(File.Exists(jsonSettingsNewFilePath));
        }
    }
}
