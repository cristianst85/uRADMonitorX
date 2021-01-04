using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using uRADMonitorX.Commons;

namespace uRADMonitorX.IntegrationTests.uRADMonitor
{
    [Ignore("IntegrationTest")]
    [TestFixture]
    [SuppressMessage(category: "Style", checkId: "IDE1006")]
    public class uRADMonitorHttpClientTests
    {
        [Test]
        public void Get()
        {
            var httpClient = new uRADMonitorHttpClient(Program.Settings.UserAgent, null, null);

            var filePath = @"..\..\..\uRADMonitorX.Tests.Files\api\devices.json";

            var fullFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            File.WriteAllText(fullFilePath, httpClient.Get(Program.Settings.uRADMonitorEndpointUrl));

            Assert.That(File.Exists(fullFilePath));
        }
    }
}
