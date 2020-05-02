using NUnit.Framework;
using uRADMonitorX.Commons;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.IntegrationTests.Core.Device
{
    [TestFixture]
    public class DeviceDataHttpReaderTest
    {
        [TestCase("10.10.0.103")]
        public void Read(string ipAddress)
        {
            var deviceDataReader = new DeviceDataHttpReader(new HttpClient(Program.UserAgent), ipAddress);

            Assert.DoesNotThrow(() => deviceDataReader.Read());
        }
    }
}
