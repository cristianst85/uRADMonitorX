using System;
using NUnit.Framework;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Tests.Core.Device {

    [TestFixture]
    public class DeviceDataFileReaderTest {

        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\10.10.0.144.htm", "41000006", 4, 108, 112, "SBM20", 13, "18,96", "35,00", 100764, 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\10.10.0.144-no-pressure-sensor.htm", "41000006", 4, 108, 112, "SBM20", 13, "18,96", "35,00", null, 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\10.10.0.144-negative-temp.htm", "41000006", 4, 108, 112, "SBM20", 13, "18,96", "-5,00", 100764, 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\fw110.htm", "41000006", 4, 108, 112, "SI29-BG", 13, "18,96", "35,00", null, 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        public void Read(String filePath, String deviceId, int deviceType, int hwVersion, int fwVersion, String detector, int radiation, String radiationAverage, String temperature, int? pressure, int voltage, int voltagePercent, int uptime, int wdt, String ip, String serverIp, String serverResponseCode) {
            IDeviceDataReader deviceDataReader = new DeviceDataFileReader(filePath);
            DeviceData deviceData = deviceDataReader.Read();
            Assert.AreEqual(deviceId, deviceData.DeviceInformation.DeviceID);
            Assert.AreEqual(hwVersion, deviceData.DeviceInformation.HardwareVersion);
            Assert.AreEqual(fwVersion, deviceData.DeviceInformation.FirmwareVersion);
            Assert.AreEqual(DeviceModelType.A2, deviceData.DeviceInformation.DeviceModel);
            Assert.AreEqual(detector, deviceData.DeviceInformation.Detector);
            Assert.AreEqual(radiation, deviceData.Radiation);
            Assert.AreEqual(double.Parse(radiationAverage), deviceData.RadiationAverage);
            Assert.AreEqual(double.Parse(temperature), deviceData.Temperature);
            Assert.AreEqual(pressure, deviceData.Pressure);
            Assert.AreEqual(voltage, deviceData.Voltage);
            Assert.AreEqual(voltagePercent, deviceData.VoltagePercent);
            Assert.AreEqual(wdt, deviceData.WDT);
            Assert.AreEqual(uptime, deviceData.Uptime);
            Assert.AreEqual(ip, deviceData.IPAddress);
            Assert.AreEqual(serverIp, deviceData.ServerIPAddress);
            Assert.AreEqual(serverResponseCode, deviceData.ServerResponseCode);
        }
    }
}
