using NUnit.Framework;
using System.IO;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Tests.Core.Device
{
    [TestFixture]
    public class DeviceDataFileReaderTest
    {
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\10.10.0.144.htm", "41000006", 4, 108, 112, DeviceModelType.A2, "SBM20", 13, "18,96", "35,00", "100764", 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\10.10.0.144-no-pressure-sensor.htm", "41000006", 4, 108, 112, DeviceModelType.A2, "SBM20", 13, "18,96", "35,00", null, 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\10.10.0.144-negative-temp.htm", "41000006", 4, 108, 112, DeviceModelType.A2, "SBM20", 13, "18,96", "-5,00", "100764", 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\fw110.htm", "41000006", 4, 108, 112, DeviceModelType.A2, "SI29-BG", 13, "18,96", "35,00", null, 377, 45, 235136, 56, "10.10.0.144", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\a3-fw118.html", "8200000B", 8, 103, 118, DeviceModelType.A3, "SI29BG", 6, "15,07", "23,10", "99890,10", 386, 20, 904, 3, "192.168.2.105", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\a3-fw119.html", "8200000F", 8, 103, 119, DeviceModelType.A3, "SI29BG", 13, null, "36,52", "101718", 380, 21, 402800, 18, "192.168.254.11", "23.239.13.18", "200")]
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\samples\fw138a.html", "110000E7", 1, 109, 138, DeviceModelType.A, "SBM20", 6, null, "19,50", null, 381, 44, 6407, 48, "192.168.4.155", "45.79.179.145", "200")]
        public void Read(string filePath, string deviceId, int deviceType, int hwVersion, int fwVersion, DeviceModelType deviceModelType, string detector, int radiation, string radiationAverage, string temperature, string pressure, int voltage, int voltagePercent, int uptime, int wdt, string ip, string serverIp, string serverResponseCode)
        {
            var fullFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            Assert.That(File.Exists(fullFilePath));

            IDeviceDataReader deviceDataReader = new DeviceDataFileReader(fullFilePath);
            DeviceData deviceData = deviceDataReader.Read();
            Assert.AreEqual(deviceId, deviceData.DeviceInformation.DeviceID);
            Assert.AreEqual(hwVersion, deviceData.DeviceInformation.HardwareVersion);
            Assert.AreEqual(fwVersion, deviceData.DeviceInformation.FirmwareVersion);
            Assert.AreEqual(deviceModelType, deviceData.DeviceInformation.DeviceModel);
            Assert.AreEqual(detector, deviceData.DeviceInformation.Detector);
            Assert.AreEqual(radiation, deviceData.Radiation);
            Assert.AreEqual(radiationAverage != null ? double.Parse(radiationAverage) : (double?)null, deviceData.RadiationAverage);
            Assert.AreEqual(double.Parse(temperature), deviceData.Temperature);
            Assert.AreEqual(pressure != null ? double.Parse(pressure) : (double?)null, deviceData.Pressure);
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
