using System;
using NUnit.Framework;
using uRADMonitorX.Updater;

namespace uRADMonitorX.Tests.Updater {

    [TestFixture]
    public class ApplicationUpdateInfoTests {

        [TestCase("0.1.2", "0.1.1", false)]
        [TestCase("0.1.2", "0.1.2", false)]
        [TestCase("0.1.2", "0.1.3", true)]
        [TestCase("0.1.2", "1.0.0", true)]
        public void IsNewVersionAvailable(String currentVersion, String availableVersion, bool isNewVersionAvailable) {
            ApplicationUpdateInfo applicationUpdateInfo = new ApplicationUpdateInfo() {
                Version = availableVersion,
                DownloadUrl = "https://mydomain.com",
                DownloadPage = "https://mydomain.com",
                FileHash = "d41d8cd98f00b204e9800998ecf8427e"
            };
            Assert.AreEqual(isNewVersionAvailable, applicationUpdateInfo.IsNewVersionAvailable(new Version(currentVersion)));
        }
    }
}
