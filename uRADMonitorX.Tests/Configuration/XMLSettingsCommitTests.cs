using System;
using System.IO;
using NUnit.Framework;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX.Tests.Configuration {

    [TestFixture]
    public class XMLSettingsCommitTests {

        private String filePath = Path.GetFullPath(@"..\..\..\Workspace\Settings.xml");

        [SetUp]
        public void SetUp() {
            Assert.IsFalse(File.Exists(filePath));
        }

        [TearDown]
        public void TearDown() {
            Assert.IsTrue(File.Exists(filePath));
            File.Delete(filePath);
            Assert.IsFalse(File.Exists(filePath));
        }

        [Test]
        public void CommitWithoutModify() {
            XMLSettings.CreateFile(filePath);
            Assert.IsTrue(File.Exists(filePath));
            ISettings settings = XMLSettings.LoadFromFile(filePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(DefaultSettings.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(DefaultSettings.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.HasPressureSensor, settings.HasPressureSensor);
            Assert.AreEqual(DefaultSettings.DeviceIPAddress, settings.DeviceIPAddress);
            Assert.AreEqual(DefaultSettings.TemperatureUnitType, settings.TemperatureUnitType);
            Assert.AreEqual(DefaultSettings.PressureUnitType, settings.PressureUnitType);
            Assert.AreEqual(DefaultSettings.PollingType, settings.PollingType);
            Assert.AreEqual(DefaultSettings.PollingInterval, settings.PollingInterval);
            Assert.AreEqual(DefaultSettings.IsPollingEnabled, settings.IsPollingEnabled);
            settings.Commit();
            settings = XMLSettings.LoadFromFile(filePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(DefaultSettings.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(DefaultSettings.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.HasPressureSensor, settings.HasPressureSensor);
            Assert.AreEqual(DefaultSettings.DeviceIPAddress, settings.DeviceIPAddress);
            Assert.AreEqual(DefaultSettings.TemperatureUnitType, settings.TemperatureUnitType);
            Assert.AreEqual(DefaultSettings.PressureUnitType, settings.PressureUnitType);
            Assert.AreEqual(DefaultSettings.PollingType, settings.PollingType);
            Assert.AreEqual(DefaultSettings.PollingInterval, settings.PollingInterval);
            Assert.AreEqual(DefaultSettings.IsPollingEnabled, settings.IsPollingEnabled);
        }

        [Test]
        public void CommitWithModify() {
            XMLSettings.CreateFile(filePath);
            Assert.IsTrue(File.Exists(filePath));
            ISettings settings = XMLSettings.LoadFromFile(filePath);
            Assert.AreEqual(DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(DefaultSettings.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(DefaultSettings.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(DefaultSettings.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(DefaultSettings.HasPressureSensor, settings.HasPressureSensor);
            Assert.AreEqual(DefaultSettings.DeviceIPAddress, settings.DeviceIPAddress);
            Assert.AreEqual(DefaultSettings.TemperatureUnitType, settings.TemperatureUnitType);
            Assert.AreEqual(DefaultSettings.PressureUnitType, settings.PressureUnitType);
            Assert.AreEqual(DefaultSettings.PollingType, settings.PollingType);
            Assert.AreEqual(DefaultSettings.PollingInterval, settings.PollingInterval);
            Assert.AreEqual(DefaultSettings.IsPollingEnabled, settings.IsPollingEnabled);
            // Modify values
            settings.StartWithWindows = !settings.StartWithWindows;
            settings.StartMinimized = !settings.StartMinimized;
            settings.ShowInTaskbar = !settings.ShowInTaskbar;
            settings.CloseToSystemTray = !settings.CloseToSystemTray;
            settings.HasPressureSensor = !settings.HasPressureSensor;
            settings.DeviceIPAddress = "127.0.0.1";
            settings.TemperatureUnitType = Core.TemperatureUnitType.Fahrenheit;
            settings.PressureUnitType = Core.PressureUnitType.kPa;
            settings.PollingType = Core.PollingType.WDTSync;
            settings.PollingInterval = DefaultSettings.PollingInterval + 5;
            settings.IsPollingEnabled = !settings.IsPollingEnabled;
            settings.Commit();
            // Reload and verify values.
            settings = XMLSettings.LoadFromFile(filePath);
            Assert.AreEqual(!DefaultSettings.StartWithWindows, settings.StartWithWindows);
            Assert.AreEqual(!DefaultSettings.StartMinimized, settings.StartMinimized);
            Assert.AreEqual(!DefaultSettings.ShowInTaskbar, settings.ShowInTaskbar);
            Assert.AreEqual(!DefaultSettings.CloseToSystemTray, settings.CloseToSystemTray);
            Assert.AreEqual(!DefaultSettings.HasPressureSensor, settings.HasPressureSensor);
            Assert.AreEqual("127.0.0.1", settings.DeviceIPAddress);
            Assert.AreEqual(TemperatureUnitType.Fahrenheit, settings.TemperatureUnitType);
            Assert.AreEqual(PressureUnitType.kPa, settings.PressureUnitType);
            Assert.AreEqual(PollingType.WDTSync, settings.PollingType);    
            Assert.AreEqual(DefaultSettings.PollingInterval + 5, settings.PollingInterval);
            Assert.AreEqual(!DefaultSettings.IsPollingEnabled, settings.IsPollingEnabled);
        }
    }
}
