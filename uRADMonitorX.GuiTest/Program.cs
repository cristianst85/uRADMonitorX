using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;
using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Infrastructure;
using uRADMonitorX.uRADMonitor.Services;

namespace uRADMonitorX.GuiTest
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyUtils.AssemblyResolver);
            InternalMain(args);
        }

        static void InternalMain(string[] args)
        {
            var settings = new InMemorySettings()
            {
                CloseToSystemTray = true,
                HasPressureSensor = true,
                TemperatureUnitType = TemperatureUnitType.Celsius,
                RadiationUnitType = RadiationUnitType.Cpm,
                PollingType = PollingType.FixedInterval,
                LastWindowXPos = DefaultSettings.LastWindowXPos,
                LastWindowYPos = DefaultSettings.LastWindowYPos,
                PollingInterval = 1,
                DeviceIPAddress = "127.0.0.1",
                IsPollingEnabled = true,
                AreNotificationsEnabled = true,
                DetectorName = "SBM20",
                HighTemperatureNotificationValue = 25,
                TemperatureNotificationUnitType = TemperatureUnitType.Celsius,
                RadiationNotificationValue = 0,
                RadiationNotificationUnitType = RadiationUnitType.Cpm
            };

            var readings = new Collection<DeviceReadings>
            {
                new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 26, Pressure = 100040, Voltage = 375, VoltagePercent = 50 },
                new DeviceReadings() { Radiation = 20, RadiationAverage = 15, Temperature = 25, Pressure = 100000, Voltage = 375, VoltagePercent = 50 },
                new DeviceReadings() { Radiation = 25, RadiationAverage = 15, Temperature = 24, Pressure = 100020, Voltage = 375, VoltagePercent = 50 },
                new DeviceReadings() { Radiation = 17, RadiationAverage = 15, Temperature = 25, Pressure = 100025, Voltage = 375, VoltagePercent = 50 },
                new DeviceReadings() { Radiation = 18, RadiationAverage = 15, Temperature = 26, Pressure = 100030, Voltage = 375, VoltagePercent = 50 },
                new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 25, Pressure = 100040, Voltage = 375, VoltagePercent = 50 }
            };

            var virtualDevice = new VirtualDevice("10000000", RadiationDetector.SBM20, 112, 108, DeviceModelType.A2, settings.DeviceIPAddress, "0.0.0.0", readings)
            {
                ServerResponseCode = HttpStatus.OK
            };

            virtualDevice.Start();

            var deviceDataReaderFactory = new DeviceDataVirtualReaderFactory(virtualDevice);
            var deviceDataJobFactory = new DeviceDataJobFactory(settings, deviceDataReaderFactory);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var logger = new NullLogger();

            var deviceDataClientConfiguration = new DeviceDataClientConfiguration();
            var httpClientConfiguration = new HttpClientConfiguration();

            var httpClientFactory = new uRADMonitorHttpClientFactory(httpClientConfiguration);
            var deviceDataClientFactory = new DeviceDataHttpClientFactory(deviceDataClientConfiguration, httpClientFactory);
            var deviceServiceFactory = new DeviceServiceFactory(new DeviceFactory(), deviceDataClientFactory);

            var formMain = new FormMain(deviceDataReaderFactory, deviceDataJobFactory, deviceServiceFactory, settings, logger);
            formMain.SettingsChangedEventHandler += new SettingsChangedEventHandler(FormMain_SettingsChangedEventHandler);

            Application.Run(formMain);
        }

        private static void FormMain_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e)
        {
            Debug.WriteLine($"[uRADMonitorX.GuiTest] [{nameof(Program)}] FormMain_SettingsChangedEventHandler()");
        }
    }
}
