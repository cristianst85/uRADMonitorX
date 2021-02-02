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
using uRADMonitorX.Helpers;
using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Infrastructure;
using uRADMonitorX.uRADMonitor.Services;
using uRADMonitorX.Windows;

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
            if (!EnvironmentUtils.IsUnix())
            {
                if (EnvironmentUtils.IsAtLeastWindows10())
                {
                    NativeMethods.SetProcessDpiAwareness(NativeMethods.ProcessDpiAwareness.ProcessSystemDpiAware);
                }
                else if (EnvironmentUtils.IsAtLeastWindowsVista())
                {
                    NativeMethods.SetProcessDPIAware();
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var settings = new InMemorySettings
            {
                IsPollingEnabled = true
            };

            settings.Display.CloseToSystemTray = true;
            settings.Display.WindowPosition = DefaultSettings.Display.WindowPosition;

            settings.Notifications.IsEnabled = true;
            settings.Notifications.TemperatureThreshold.HighValue = 25;
            settings.Notifications.TemperatureThreshold.MeasurementUnit = TemperatureUnitType.Celsius;
            settings.Notifications.RadiationThreshold.HighValue = 0;
            settings.Notifications.RadiationThreshold.MeasurementUnit = RadiationUnitType.Cpm;

            settings.Misc.TemperatureUnitType = TemperatureUnitType.Celsius;
            settings.Misc.PressureUnitType = PressureUnitType.Pa;
            settings.Misc.RadiationUnitType = RadiationUnitType.Cpm;

            var deviceSettings = new DeviceSettings
            {
                EndpointUrl = "127.0.0.1"
            };

            deviceSettings.Polling.IsEnabled = true;
            deviceSettings.Polling.Type = PollingType.FixedInterval;
            deviceSettings.Polling.Interval = 1;

            settings.Devices.Add(deviceSettings);

            var readings = new Collection<DeviceReadings>
            {
                new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 26, Pressure = 100040, Voltage = 375, VoltagePercent = 50 },
                new DeviceReadings() { Radiation = 20, RadiationAverage = 15, Temperature = 25, Pressure = 100000, Voltage = 377, VoltagePercent = 44 },
                new DeviceReadings() { Radiation = 25, RadiationAverage = 15, Temperature = 24, Pressure = 100020, Voltage = 378, VoltagePercent = 46 },
                new DeviceReadings() { Radiation = 17, RadiationAverage = 15, Temperature = 25, Pressure = 100025, Voltage = 400, VoltagePercent = 43 },
                new DeviceReadings() { Radiation = 18, RadiationAverage = 15, Temperature = 26, Pressure = 100030, Voltage = 365, VoltagePercent = 45 },
                new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 25, Pressure = 100040, Voltage = 368, VoltagePercent = 47 }
            };

            var virtualDevice = new VirtualDevice("10000000", RadiationDetector.SBM20, 112, 108, DeviceModelType.A2, deviceSettings.EndpointUrl, readings)
            {
                ServerResponseCode = HttpStatus.OK
            };

            virtualDevice.Start();

            var deviceDataReaderFactory = new DeviceDataVirtualReaderFactory(virtualDevice);
            var deviceDataJobFactory = new DeviceDataJobFactory(settings, deviceDataReaderFactory);

            var logger = new NullLogger();

            var deviceDataClientConfiguration = new DeviceDataClientConfiguration();
            var httpClientConfiguration = new HttpClientConfiguration();

            var httpClientFactory = new uRADMonitorHttpClientFactory(httpClientConfiguration);
            var deviceDataClientFactory = new DeviceDataHttpClientFactory(deviceDataClientConfiguration, httpClientFactory);
            var deviceServiceFactory = new DeviceServiceFactory(new DeviceFactory(), deviceDataClientFactory);

            var formMain = new FormMain(deviceDataReaderFactory, deviceDataJobFactory, deviceServiceFactory, settings, logger);
            formMain.SettingsChangedEventHandler += new SettingsChangedEventHandler(FormMain_SettingsChangedEventHandler);

            // Both uRADMonitor API and GitHub API requires TLS v1.2.
            ServicePointManagerHelper.SetSecurityProtocolToTls12();

            Application.Run(formMain);
        }

        private static void FormMain_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e)
        {
            Debug.WriteLine($"[uRADMonitorX.GuiTest] [{nameof(Program)}] FormMain_SettingsChangedEventHandler()");
        }
    }
}
