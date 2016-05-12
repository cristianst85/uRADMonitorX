using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.GuiTest {

    static class Program {

        [STAThread]
        static void Main(string[] args) {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyUtils.AssemblyResolver);
            internalMain(args);
        }

        static void internalMain(string[] args) {

            ISettings settings = new InMemorySettings() {
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

            ILogger logger = new NullLogger();

            ICollection<DeviceReadings> readings = new Collection<DeviceReadings>();
            readings.Add(new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 26, Pressure = 100040, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 20, RadiationAverage = 15, Temperature = 25, Pressure = 100000, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 25, RadiationAverage = 15, Temperature = 24, Pressure = 100020, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 17, RadiationAverage = 15, Temperature = 25, Pressure = 100025, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 18, RadiationAverage = 15, Temperature = 26, Pressure = 100030, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 25, Pressure = 100040, Voltage = 375, VoltagePercent = 50 });

            VirtualDevice virtualDevice = new VirtualDevice("10000000", RadiationDetector.SBM20, 112, 108, DeviceModelType.A2, settings.DeviceIPAddress, "0.0.0.0", readings);
            virtualDevice.ServerResponseCode = HttpStatus.OK;
            virtualDevice.Start();

            IDeviceDataReaderFactory deviceDataReaderFactory = new DeviceDataVirtualReaderFactory(virtualDevice);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormMain formMain = new FormMain(deviceDataReaderFactory, settings, logger);
            formMain.SettingsChangedEventHandler += new SettingsChangedEventHandler(formMain_SettingsChangedEventHandler);

            Application.Run(formMain);
        }

        private static void formMain_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e) {
            Debug.WriteLine("Settings changed.");
        }
    }
}
