using System;
using System.IO;
using NUnit.Framework;
using uRADMonitorX.Configuration;

namespace uRADMonitorX.Tests.Configuration {

    [TestFixture]
    public class XMLSettingsTests {

        private String outputfilePath = Path.GetFullPath(@"..\..\..\uRADMonitorX.Tests.Files\configs\Settings.xml");

        [SetUp]
        public void SetUp() {
            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [TearDown]
        public void TearDown() {
            Assert.IsTrue(File.Exists(outputfilePath));
            File.Delete(outputfilePath);
            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [Test]
        public void CreateFileWithDefaults() {
            XMLSettings.CreateFile(outputfilePath);
            Assert.IsTrue(File.Exists(outputfilePath));
            XMLSettings settings = XMLSettings.LoadFromFile(outputfilePath);
            Assert.AreEqual(outputfilePath, settings.FilePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);

            Assert.AreEqual(DefaultSettings.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(DefaultSettings.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(DefaultSettings.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(DefaultSettings.LastWindowXPos, settings.LastWindowXPos);
            Assert.AreEqual(DefaultSettings.LastWindowYPos, settings.LastWindowYPos);

            Assert.AreEqual(DefaultSettings.IsLoggingEnabled, settings.IsLoggingEnabled);
            Assert.AreEqual(DefaultSettings.LogDirectoryPath, settings.LogDirectoryPath);

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
        }
    }
}