using System;
using NUnit.Framework;
using uRADMonitorX.Updater;

namespace uRADMonitorX.IntegrationTests.Updater {

    [TestFixture]
    public class GitHubApplicationUpdaterTests {

        [TestCase("https://api.github.com/repos/cristianst85/uRADMonitorX/releases/latest")]
        public void Check(String updaterUrl) {
            IHttpApplicationUpdater applicationUpdater = new GitHubApplicationUpdater(updaterUrl);
            Assert.DoesNotThrow(() => { applicationUpdater.Check(); });
        }
    }
}
