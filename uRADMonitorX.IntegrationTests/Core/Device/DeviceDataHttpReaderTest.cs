﻿using NUnit.Framework;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.IntegrationTests.Core.Device
{
    [TestFixture]
    public class DeviceDataHttpReaderTest
    {
        [TestCase("10.10.0.103")]
        public void Read(string ipAddress)
        {
            IDeviceDataReader deviceDataReader = new DeviceDataHttpReader(ipAddress);
            Assert.DoesNotThrow(() => deviceDataReader.Read());
        }
    }
}
