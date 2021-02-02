using NUnit.Framework;
using System.IO;
using uRADMonitorX.Configuration;

namespace uRADMonitorX.Tests.Configuration
{
    [TestFixture]
    public class XMLSettingsTests
    {
        private readonly string xmlSettingsFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\uRADMonitorX.Tests.Files\configs\Settings.xml"));

        [SetUp]
        public void SetUp()
        {
            Assert.IsFalse(File.Exists(xmlSettingsFilePath));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(xmlSettingsFilePath);

            Assert.IsFalse(File.Exists(xmlSettingsFilePath));
        }

        [Test]
        public void CreateFileWithDefaults()
        {
            XMLSettings.CreateFile(xmlSettingsFilePath);

            Assert.IsTrue(File.Exists(xmlSettingsFilePath));

            XMLSettings settings = XMLSettings.LoadFromFile(xmlSettingsFilePath);

            Assert.AreEqual(xmlSettingsFilePath, settings.FilePath);

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
    }
}
