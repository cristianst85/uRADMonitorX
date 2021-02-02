using NUnit.Framework;
using System.IO;
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

            Assert.AreEqual(xmlSettings.StartWithWindows, jsonSettings.StartWithWindows);
            Assert.AreEqual(xmlSettings.AutomaticallyCheckForUpdates, jsonSettings.AutomaticallyCheckForUpdates);

            Assert.AreEqual(xmlSettings.StartMinimized, jsonSettings.StartMinimized);
            Assert.AreEqual(xmlSettings.ShowInTaskbar, jsonSettings.ShowInTaskbar);
            Assert.AreEqual(xmlSettings.CloseToSystemTray, jsonSettings.CloseToSystemTray);
            Assert.AreEqual(xmlSettings.LastWindowXPos, jsonSettings.LastWindowXPos);
            Assert.AreEqual(xmlSettings.LastWindowYPos, jsonSettings.LastWindowYPos);

            Assert.AreEqual(xmlSettings.IsLoggingEnabled, jsonSettings.IsLoggingEnabled);
            Assert.AreEqual(xmlSettings.LogDirectoryPath, jsonSettings.LogDirectoryPath);
            Assert.AreEqual(xmlSettings.IsDataLoggingEnabled, jsonSettings.IsDataLoggingEnabled);
            Assert.AreEqual(xmlSettings.DataLoggingToSeparateFile, jsonSettings.DataLoggingToSeparateFile);
            Assert.AreEqual(xmlSettings.DataLogDirectoryPath, jsonSettings.DataLogDirectoryPath);

            Assert.AreEqual(xmlSettings.DetectorName, jsonSettings.DetectorName);
            Assert.AreEqual(xmlSettings.HasPressureSensor, jsonSettings.HasPressureSensor);
            Assert.AreEqual(xmlSettings.DeviceIPAddress, jsonSettings.DeviceIPAddress);

            Assert.AreEqual(xmlSettings.TemperatureUnitType, jsonSettings.TemperatureUnitType);
            Assert.AreEqual(xmlSettings.PressureUnitType, jsonSettings.PressureUnitType);
            Assert.AreEqual(xmlSettings.RadiationUnitType, jsonSettings.RadiationUnitType);

            Assert.AreEqual(xmlSettings.PollingType, jsonSettings.PollingType);
            Assert.AreEqual(xmlSettings.PollingInterval, jsonSettings.PollingInterval);
            Assert.AreEqual(xmlSettings.IsPollingEnabled, jsonSettings.IsPollingEnabled);

            Assert.AreEqual(xmlSettings.AreNotificationsEnabled, jsonSettings.AreNotificationsEnabled);
            Assert.AreEqual(xmlSettings.HighTemperatureNotificationValue, jsonSettings.HighTemperatureNotificationValue);
            Assert.AreEqual(xmlSettings.RadiationNotificationValue, jsonSettings.RadiationNotificationValue);
            Assert.AreEqual(xmlSettings.TemperatureNotificationUnitType, jsonSettings.TemperatureNotificationUnitType);
            Assert.AreEqual(xmlSettings.RadiationNotificationUnitType, jsonSettings.RadiationNotificationUnitType);

            Assert.AreEqual(xmlSettings.uRADMonitorAPIUserId, jsonSettings.uRADMonitorAPIUserId);
            Assert.AreEqual(xmlSettings.uRADMonitorAPIUserKey, jsonSettings.uRADMonitorAPIUserKey);
        }

        [Test]
        public void LoadFromXmlFile_Enum_SerializationDeserialization()
        {
            XMLSettings.CreateFile(xmlSettingsFilePath);

            Assert.IsTrue(File.Exists(xmlSettingsFilePath));

            var xmlSettings = XMLSettings.LoadFromFile(xmlSettingsFilePath);

            Assert.AreEqual(xmlSettingsFilePath, xmlSettings.FilePath);

            xmlSettings.TemperatureUnitType = uRADMonitorX.Core.TemperatureUnitType.Fahrenheit;
            xmlSettings.RadiationUnitType = uRADMonitorX.Core.RadiationUnitType.uSvH;
            xmlSettings.Commit();

            var jsonSettings = JsonSettings.LoadFromXmlFile(xmlSettingsFilePath);

            Assert.AreNotEqual(xmlSettings.FilePath, jsonSettings.FilePath);
            Assert.AreEqual(".json", Path.GetExtension(jsonSettings.FilePath));

            Assert.AreEqual(xmlSettings.TemperatureNotificationUnitType, jsonSettings.TemperatureNotificationUnitType);
            Assert.AreEqual(xmlSettings.RadiationUnitType, jsonSettings.RadiationUnitType);
        }

        [Test]
        public void CreateFileWithDefaults()
        {
            JsonSettings.CreateFile(jsonSettingsFilePath);

            Assert.IsTrue(File.Exists(jsonSettingsFilePath));

            var settings = JsonSettings.LoadFromFile(jsonSettingsFilePath);

            Assert.AreEqual(jsonSettingsFilePath, settings.FilePath);

            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.AutomaticallyCheckForUpdates, settings.AutomaticallyCheckForUpdates);

            Assert.AreEqual(DefaultSettings.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(DefaultSettings.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(DefaultSettings.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(DefaultSettings.LastWindowXPos, settings.LastWindowXPos);
            Assert.AreEqual(DefaultSettings.LastWindowYPos, settings.LastWindowYPos);

            Assert.AreEqual(DefaultSettings.IsLoggingEnabled, settings.IsLoggingEnabled);
            Assert.AreEqual(DefaultSettings.LogDirectoryPath, settings.LogDirectoryPath);
            Assert.AreEqual(DefaultSettings.IsDataLoggingEnabled, settings.IsDataLoggingEnabled);
            Assert.AreEqual(DefaultSettings.DataLoggingToSeparateFile, settings.DataLoggingToSeparateFile);
            Assert.AreEqual(DefaultSettings.DataLogDirectoryPath, settings.DataLogDirectoryPath);

            Assert.AreEqual(DefaultSettings.DetectorName, settings.DetectorName);
            Assert.AreEqual(DefaultSettings.HasPressureSensor, settings.HasPressureSensor);
            Assert.AreEqual(DefaultSettings.DeviceIPAddress, settings.DeviceIPAddress);

            Assert.AreEqual(DefaultSettings.TemperatureUnitType, settings.TemperatureUnitType);
            Assert.AreEqual(DefaultSettings.PressureUnitType, settings.PressureUnitType);
            Assert.AreEqual(DefaultSettings.RadiationUnitType, settings.RadiationUnitType);

            Assert.AreEqual(DefaultSettings.PollingType, settings.PollingType);
            Assert.AreEqual(DefaultSettings.PollingInterval, settings.PollingInterval);
            Assert.AreEqual(DefaultSettings.IsPollingEnabled, settings.IsPollingEnabled);

            Assert.AreEqual(DefaultSettings.AreNotificationsEnabled, settings.AreNotificationsEnabled);
            Assert.AreEqual(DefaultSettings.HighTemperatureNotificationValue, settings.HighTemperatureNotificationValue);
            Assert.AreEqual(DefaultSettings.RadiationNotificationValue, settings.RadiationNotificationValue);
            Assert.AreEqual(DefaultSettings.TemperatureNotificationUnitType, settings.TemperatureNotificationUnitType);
            Assert.AreEqual(DefaultSettings.RadiationNotificationUnitType, settings.RadiationNotificationUnitType);

            Assert.IsNull(settings.uRADMonitorAPIUserId);
            Assert.IsNull(settings.uRADMonitorAPIUserId);
        }

        [Test]
        public void Commit()
        {
            JsonSettings.CreateFile(jsonSettingsFilePath);

            var jsonSettings = JsonSettings.LoadFromFile(jsonSettingsFilePath);

            Assert.IsFalse(JsonSettings.LoadFromFile(jsonSettingsFilePath).IsLoggingEnabled);

            jsonSettings.IsLoggingEnabled = true;

            jsonSettings.Commit();

            Assert.IsTrue(JsonSettings.LoadFromFile(jsonSettingsFilePath).IsLoggingEnabled);
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
