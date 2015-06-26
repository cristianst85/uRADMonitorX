using System;
using NUnit.Framework;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Tests {

    [TestFixture]
    public class DeviceDataHttpReaderTest {

        [TestCase("10.10.0.144")]
        public void Read(String ipAddress) {
            IDeviceDataReader deviceDataReader = new DeviceDataHttpReader(ipAddress);
            DeviceData deviceData = deviceDataReader.Read();
            // TODO: asserts
        }
    }
}
