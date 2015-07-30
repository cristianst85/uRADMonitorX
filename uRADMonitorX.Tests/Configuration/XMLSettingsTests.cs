using System;
using System.IO;
using NUnit.Framework;
using uRADMonitorX.Configuration;

namespace uRADMonitorX.Tests.Configuration {

    [TestFixture]
    public class XMLSettingsTests {

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
        public void CreateFile() {
            XMLSettings.CreateFile(filePath);
            Assert.IsTrue(File.Exists(filePath));
            int fileSize = File.ReadAllBytes(filePath).Length;
            Console.WriteLine(String.Format("File size: {0}", fileSize));
            Assert.IsTrue(fileSize > 0);
        }

        [Test]
        public void CreateFileTestDefaults() {
            XMLSettings.CreateFile(filePath);
            Assert.IsTrue(File.Exists(filePath));
            XMLSettings settings = XMLSettings.LoadFromFile(filePath);
            Assert.AreEqual(filePath, settings.FilePath);
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
        }
    }
}