using NUnit.Framework;
using uRADMonitorX.Updater;

namespace uRADMonitorX.IntegrationTests.Updater
{
    [TestFixture]
    public class GitHubUpdaterTests
    {
        public void Check()
        {
            Assert.DoesNotThrow(() => { new GitHubUpdater().Check(Program.UpdateUrl); });
        }
    }
}
