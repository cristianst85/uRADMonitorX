using NUnit.Framework;
using System.IO;
using System.Linq;
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

            Assert.IsEmpty(settings.Devices.Single().GetRadiationDetectorName());
            Assert.IsEmpty(settings.Devices.Single().EndpointUrl);

            Assert.AreEqual(DeviceCapability.Temperature | DeviceCapability.Radiation, settings.Devices.Single().GetDeviceCapabilities());

            Assert.IsNull(settings.uRADMonitorNetwork.UserId);
            Assert.IsNull(settings.uRADMonitorNetwork.UserKey);
        }
    }
}
