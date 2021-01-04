using NUnit.Framework;
using uRADMonitorX.Updater;

namespace uRADMonitorX.IntegrationTests.Updater
{
    [Ignore("IntegrationTest")]
    [TestFixture]
    public class GitHubUpdaterTests
    {
        [TestCase]
        public void Check()
        {
            Assert.DoesNotThrow(() => { new GitHubUpdater().Check(Program.UpdateUrl); });
        }
    }
}
