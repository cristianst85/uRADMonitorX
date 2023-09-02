using Moq;
using NUnit.Framework;
using System.IO;
using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Infrastructure;
using uRADMonitorX.uRADMonitor.Services;

namespace uRADMonitorX.Tests.uRADMonitor.Services
{
    [TestFixture]
    public class DeviceServiceTests
    {
        [TestCase(@"..\..\..\uRADMonitorX.Tests.Files\api\devices.json", 2010)]
        public void GetAll(string filePath, int expectedDevicesCount)
        {
            var fullFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            Assert.That(File.Exists(fullFilePath));

            var fileContent = File.ReadAllText(fullFilePath);

            var deviceDataClientMock = new Mock<IDeviceDataClient>();
            deviceDataClientMock.Setup(x => x.Get()).Returns(fileContent);

            var deviceService = new DeviceService(deviceDataClientMock.Object, new DeviceFactory());

            DeviceServiceResponse response = null;

            Assert.DoesNotThrow(() =>
            {
                response = deviceService.GetAll();
            });

            Assert.IsFalse(response.HasError);
            Assert.That(response.Devices.Count, Is.EqualTo(expectedDevicesCount));
        }

        [TestCase("{ \"error\": \"Authentication failed\" }")]
        public void GetAllReturnsApiError(string json)
        {
            var deviceDataClientMock = new Mock<IDeviceDataClient>();
            deviceDataClientMock.Setup(x => x.Get()).Returns(json);

            var deviceService = new DeviceService(deviceDataClientMock.Object, new DeviceFactory());

            DeviceServiceResponse response = null;

            Assert.DoesNotThrow(() =>
            {
                response = deviceService.GetAll();
            });

            Assert.IsTrue(response.HasError);
            Assert.That(response.Error, Is.EqualTo("Authentication failed"));
        }
    }
}
