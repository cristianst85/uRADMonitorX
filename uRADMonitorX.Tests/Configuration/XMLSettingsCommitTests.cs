using System;
using System.IO;
using NUnit.Framework;
using uRADMonitorX.Commons;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX.Tests.Configuration {

    [TestFixture]
    public class XMLSettingsCommitTests {

        private String outputfilePath = Path.GetFullPath(@"..\..\..\uRADMonitorX.Tests.Files\configs\Settings.xml");

        [SetUp]
        public void SetUp() {
            var directory = Path.GetDirectoryName(outputfilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [TearDown]
        public void TearDown() {
            Assert.IsTrue(File.Exists(outputfilePath));
            File.Delete(outputfilePath);
            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [Test]
        public void CommitWithoutModify() {
            XMLSettings.CreateFile(outputfilePath);
            Assert.IsTrue(File.Exists(outputfilePath));
            ISettings settings = XMLSettings.LoadFromFile(outputfilePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.AutomaticallyCheckForUpdates, settings.AutomaticallyCheckForUpdates);
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
            settings.Commit();
            settings = XMLSettings.LoadFromFile(outputfilePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.AutomaticallyCheckForUpdates, settings.AutomaticallyCheckForUpdates);
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

        [Test]
        public void CommitWithModify() {
            XMLSettings.CreateFile(outputfilePath);
            Assert.IsTrue(File.Exists(outputfilePath));
            ISettings settings = XMLSettings.LoadFromFile(outputfilePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.AutomaticallyCheckForUpdates, settings.AutomaticallyCheckForUpdates);
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
            // Modify values
            settings.StartWithWindows = !settings.StartWithWindows;
            settings.AutomaticallyCheckForUpdates = !settings.AutomaticallyCheckForUpdates;
            settings.StartMinimized = !settings.StartMinimized;
            settings.ShowInTaskbar = !settings.ShowInTaskbar;
            settings.CloseToSystemTray = !settings.CloseToSystemTray;
            settings.LastWindowXPos = settings.LastWindowXPos + 100;
            settings.LastWindowYPos = settings.LastWindowYPos + 150;
            settings.IsLoggingEnabled = !settings.IsLoggingEnabled;
            settings.LogDirectoryPath = ".";
            settings.DetectorName = RadiationDetector.SBM20.ToString();
            settings.HasPressureSensor = !settings.HasPressureSensor;
            settings.DeviceIPAddress = "127.0.0.1";
            settings.TemperatureUnitType = TemperatureUnitType.Fahrenheit;
            settings.PressureUnitType = PressureUnitType.kPa;
            settings.RadiationUnitType = RadiationUnitType.uRemH;
            settings.PollingType = PollingType.WDTSync;
            settings.PollingInterval = DefaultSettings.PollingInterval + 5;
            settings.IsPollingEnabled = !settings.IsPollingEnabled;
            settings.AreNotificationsEnabled = !settings.AreNotificationsEnabled;
            settings.HighTemperatureNotificationValue = settings.HighTemperatureNotificationValue + 1;
            settings.RadiationNotificationValue = settings.RadiationNotificationValue + 1;
            settings.TemperatureNotificationUnitType = TemperatureUnitType.Fahrenheit;
            settings.RadiationNotificationUnitType = RadiationUnitType.uRemH;
            settings.Commit();
            // Reload and verify values.
            XMLSettings settingsAfterCommit = XMLSettings.LoadFromFile(outputfilePath);
            Assert.AreEqual(settingsAfterCommit.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(settingsAfterCommit.AutomaticallyCheckForUpdates, settings.AutomaticallyCheckForUpdates);
            Assert.AreEqual(settingsAfterCommit.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(settingsAfterCommit.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(settingsAfterCommit.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(settingsAfterCommit.LastWindowXPos, settings.LastWindowXPos);
            Assert.AreEqual(settingsAfterCommit.LastWindowYPos, settings.LastWindowYPos);
            Assert.AreEqual(settingsAfterCommit.IsLoggingEnabled, settings.IsLoggingEnabled);
            Assert.AreEqual(settingsAfterCommit.LogDirectoryPath, settings.LogDirectoryPath);
            Assert.AreEqual(settingsAfterCommit.DetectorName, settings.DetectorName);
            Assert.AreEqual(settingsAfterCommit.HasPressureSensor, settings.HasPressureSensor);
            Assert.AreEqual(settingsAfterCommit.DeviceIPAddress, settings.DeviceIPAddress);
            Assert.AreEqual(settingsAfterCommit.TemperatureUnitType, settings.TemperatureUnitType);
            Assert.AreEqual(settingsAfterCommit.PressureUnitType, settings.PressureUnitType);
            Assert.AreEqual(settingsAfterCommit.RadiationUnitType, settings.RadiationUnitType);
            Assert.AreEqual(settingsAfterCommit.PollingType, settings.PollingType);
            Assert.AreEqual(settingsAfterCommit.PollingInterval, settings.PollingInterval);
            Assert.AreEqual(settingsAfterCommit.IsPollingEnabled, settings.IsPollingEnabled);
            Assert.AreEqual(settingsAfterCommit.AreNotificationsEnabled, settings.AreNotificationsEnabled);
            Assert.AreEqual(settingsAfterCommit.HighTemperatureNotificationValue, settings.HighTemperatureNotificationValue);
            Assert.AreEqual(settingsAfterCommit.RadiationNotificationValue, settings.RadiationNotificationValue);
            Assert.AreEqual(settingsAfterCommit.TemperatureNotificationUnitType, settings.TemperatureNotificationUnitType);
            Assert.AreEqual(settingsAfterCommit.RadiationNotificationUnitType, settings.RadiationNotificationUnitType);
        }
    }
}
