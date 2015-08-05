using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.GuiTest {

    static class Program {

        [STAThread]
        static void Main(string[] args) {

            ISettings settings = new InMemorySettings() {
                CloseToSystemTray = true,
                HasPressureSensor = false,
                TemperatureUnitType = TemperatureUnitType.Celsius,
                RadiationUnitType = RadiationUnitType.Cpm,
                PollingType = PollingType.FixedInterval,
                PollingInterval = 1,
                DeviceIPAddress = "127.0.0.1",
                IsPollingEnabled = true
            };

            ILogger logger = new NullLogger();

            ICollection<DeviceReadings> readings = new Collection<DeviceReadings>();
            readings.Add(new DeviceReadings() { Radiation = 15, RadiationAverage = 15, Temperature = 25, Pressure = 100000, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 16, RadiationAverage = 15, Temperature = 24, Pressure = 100020, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 17, RadiationAverage = 15, Temperature = 25, Pressure = 100025, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 18, RadiationAverage = 15, Temperature = 26, Pressure = 100030, Voltage = 375, VoltagePercent = 50 });
            readings.Add(new DeviceReadings() { Radiation = 19, RadiationAverage = 15, Temperature = 25, Pressure = 100040, Voltage = 375, VoltagePercent = 50 });

            VirtualDevice virtualDevice = new VirtualDevice("10000000", RadiationDetector.SBM20, 112, 108, DeviceType.Type4, settings.DeviceIPAddress, "0.0.0.0", readings);
            virtualDevice.Start();

            IDeviceDataReaderFactory deviceDataReaderFactory = new DeviceDataVirtualReaderFactory(virtualDevice);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(deviceDataReaderFactory, settings, logger, true));
        }
    }
}
