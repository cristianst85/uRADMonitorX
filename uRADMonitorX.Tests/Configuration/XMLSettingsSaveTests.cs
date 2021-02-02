using NUnit.Framework;
using System.Drawing;
using System.IO;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Tests.Configuration
{
    [TestFixture]
    public class XMLSettingsSaveTests
    {
        private readonly string outputfilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\uRADMonitorX.Tests.Files\configs\Settings.xml"));

        [SetUp]
        public void SetUp()
        {
            var directory = Path.GetDirectoryName(outputfilePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [TearDown]
        public void TearDown()
        {
            Assert.IsTrue(File.Exists(outputfilePath));

            File.Delete(outputfilePath);

            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [Test]
        public void SaveWithoutModify()
        {
            XMLSettings.CreateFile(outputfilePath);

            Assert.IsTrue(File.Exists(outputfilePath));

            XMLSettings settings = XMLSettings.LoadFromFile(outputfilePath);

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

            Assert.AreEqual(DefaultSettings.Polling.IsEnabled, settings.Device.Polling.IsEnabled);
            Assert.AreEqual(DefaultSettings.Polling.Type, settings.Device.Polling.Type);
            Assert.AreEqual(DefaultSettings.Polling.Interval, settings.Device.Polling.Interval);
        
            Assert.IsEmpty(settings.Device.GetRadiationDetectorName());
            Assert.IsEmpty(settings.Device.EndpointUrl);

            Assert.AreEqual(DeviceCapability.Temperature | DeviceCapability.Radiation, settings.Device.GetDeviceCapabilities());

            Assert.IsNull(settings.uRADMonitorNetwork.UserId);
            Assert.IsNull(settings.uRADMonitorNetwork.UserKey);

            settings.Save();

            settings = XMLSettings.LoadFromFile(outputfilePath);

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

            Assert.AreEqual(DefaultSettings.Polling.IsEnabled, settings.Device.Polling.IsEnabled);
            Assert.AreEqual(DefaultSettings.Polling.Type, settings.Device.Polling.Type);
            Assert.AreEqual(DefaultSettings.Polling.Interval, settings.Device.Polling.Interval);

            Assert.IsEmpty(settings.Device.GetRadiationDetectorName());
            Assert.IsEmpty(settings.Device.EndpointUrl);

            Assert.AreEqual(DeviceCapability.Temperature | DeviceCapability.Radiation, settings.Device.GetDeviceCapabilities());

            Assert.IsNull(settings.uRADMonitorNetwork.UserId);
            Assert.IsNull(settings.uRADMonitorNetwork.UserKey);
        }

        [Test]
        public void SaveWithModify()
        {
            XMLSettings.CreateFile(outputfilePath);

            Assert.IsTrue(File.Exists(outputfilePath));

            XMLSettings settings = XMLSettings.LoadFromFile(outputfilePath);

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

            Assert.AreEqual(DefaultSettings.Polling.IsEnabled, settings.Device.Polling.IsEnabled);
            Assert.AreEqual(DefaultSettings.Polling.Type, settings.Device.Polling.Type);
            Assert.AreEqual(DefaultSettings.Polling.Interval, settings.Device.Polling.Interval);

            // Modify values
            settings.IsPollingEnabled = !settings.IsPollingEnabled;

            settings.General.StartWithWindows = !settings.General.StartWithWindows;
            settings.General.AutomaticallyCheckForUpdates = !settings.General.AutomaticallyCheckForUpdates;

            settings.Display.StartMinimized = !settings.Display.StartMinimized;
            settings.Display.ShowInTaskbar = !settings.Display.ShowInTaskbar;
            settings.Display.CloseToSystemTray = !settings.Display.CloseToSystemTray;

            settings.Display.WindowPosition = new Point(settings.Display.WindowPosition.X + 10, settings.Display.WindowPosition.Y + 15);

            settings.Logging.IsEnabled = !settings.Logging.IsEnabled;
            settings.Logging.DirectoryPath = ".";

            settings.Logging.DataLogging.IsEnabled = !settings.Logging.DataLogging.IsEnabled;
            settings.Logging.DataLogging.UseSeparateFile = !settings.Logging.DataLogging.UseSeparateFile;
            settings.Logging.DataLogging.DirectoryPath = @"\data";

            settings.Notifications.IsEnabled = !settings.Notifications.IsEnabled;
            settings.Notifications.TemperatureThreshold.HighValue = settings.Notifications.TemperatureThreshold.HighValue + 1;
            settings.Notifications.TemperatureThreshold.MeasurementUnit = TemperatureUnitType.Fahrenheit;
            settings.Notifications.RadiationThreshold.HighValue = settings.Notifications.RadiationThreshold.HighValue + 0.5m;
            settings.Notifications.RadiationThreshold.MeasurementUnit = RadiationUnitType.uRemH;

            settings.Misc.TemperatureUnitType = TemperatureUnitType.Fahrenheit;
            settings.Misc.PressureUnitType = PressureUnitType.kPa;
            settings.Misc.RadiationUnitType = RadiationUnitType.uRemH;

            settings.Device.EndpointUrl = "127.0.0.1";
            settings.Device.SetRadiationDetectorName(RadiationDetector.SBM20.Name);
            settings.Device.SetDeviceCapabilities(DeviceCapability.Radiation | DeviceCapability.Temperature | DeviceCapability.Pressure);

            settings.Device.Polling.IsEnabled = !settings.Device.Polling.IsEnabled;

            settings.Device.Polling.Type = settings.Device.Polling.Type == PollingType.FixedInterval ? PollingType.WDTSync : PollingType.FixedInterval;
            settings.Device.Polling.Interval = settings.Device.Polling.Interval + 5;
          
            settings.Save();

            // Reload and verify values.
            XMLSettings settingsAfterSaving = XMLSettings.LoadFromFile(outputfilePath);

            Assert.AreEqual(settingsAfterSaving.IsPollingEnabled, settings.IsPollingEnabled);

            Assert.AreEqual(settingsAfterSaving.General.StartWithWindows, settings.General.StartWithWindows);
            Assert.AreEqual(settingsAfterSaving.General.AutomaticallyCheckForUpdates, settings.General.AutomaticallyCheckForUpdates);

            Assert.AreEqual(settingsAfterSaving.Display.StartMinimized, settings.Display.StartMinimized);
            Assert.AreEqual(settingsAfterSaving.Display.ShowInTaskbar, settings.Display.ShowInTaskbar);
            Assert.AreEqual(settingsAfterSaving.Display.CloseToSystemTray, settings.Display.CloseToSystemTray);
            Assert.AreEqual(settingsAfterSaving.Display.WindowPosition, settings.Display.WindowPosition);

            Assert.AreEqual(settingsAfterSaving.Logging.IsEnabled, settings.Logging.IsEnabled);
            Assert.AreEqual(settingsAfterSaving.Logging.DirectoryPath, settings.Logging.DirectoryPath);

            Assert.AreEqual(settingsAfterSaving.Device.GetRadiationDetectorName(), settings.Device.GetRadiationDetectorName());
            Assert.AreEqual(settingsAfterSaving.Device.GetDeviceCapabilities(), settings.Device.GetDeviceCapabilities());

            Assert.AreEqual(settingsAfterSaving.Device.EndpointUrl, settings.Device.EndpointUrl);
            Assert.AreEqual(settingsAfterSaving.Device.Polling.IsEnabled, settings.Device.Polling.IsEnabled);
            Assert.AreEqual(settingsAfterSaving.Device.Polling.Type, settings.Device.Polling.Type);
            Assert.AreEqual(settingsAfterSaving.Device.Polling.Interval, settings.Device.Polling.Interval);

            Assert.AreEqual(settingsAfterSaving.Notifications.IsEnabled, settings.Notifications.IsEnabled);
            Assert.AreEqual(settingsAfterSaving.Notifications.TemperatureThreshold, settings.Notifications.TemperatureThreshold);
            Assert.AreEqual(settingsAfterSaving.Notifications.RadiationThreshold, settings.Notifications.RadiationThreshold);

            Assert.AreEqual(settingsAfterSaving.Misc.TemperatureUnitType, settings.Misc.TemperatureUnitType);
            Assert.AreEqual(settingsAfterSaving.Misc.PressureUnitType, settings.Misc.PressureUnitType);
            Assert.AreEqual(settingsAfterSaving.Misc.RadiationUnitType, settings.Misc.RadiationUnitType);
        }
    }
}
