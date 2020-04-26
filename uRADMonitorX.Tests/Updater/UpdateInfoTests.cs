using NUnit.Framework;
using System;
using uRADMonitorX.Updater;

namespace uRADMonitorX.Tests.Updater
{
    [TestFixture]
    public class UpdateInfoTests
    {
        [TestCase("0.1.2", "0.1.1", false)]
        [TestCase("0.1.2.0", "0.1.2", false)]
        [TestCase("0.1.2.1", "0.1.2", false)]
        [TestCase("0.1.2", "0.1.2", false)]
        [TestCase("0.1.2", "0.1.3", true)]
        [TestCase("0.1.2", "1.0.0", true)]
        public void IsNewVersionAvailable(string currentVersion, string availableVersion, bool expectedIsNewVersionAvailable)
        {
            var updateInfo = new UpdateInfo()
            {
                Version = new Version(availableVersion)
            };

            Assert.AreEqual(expectedIsNewVersionAvailable, updateInfo.IsNewVersionAvailable(new Version(currentVersion)));
        }

        [TestCase("0.1.2", "0.1.1", true)]
        [TestCase("0.1.2", "0.1.2", false)]
        [TestCase("0.1.2.0", "0.1.2", false)]
        [TestCase("0.1.2.1", "0.1.2", false)]
        [TestCase("0.1.2", "0.1.3", false)]
        [TestCase("0.1.2", "1.0.0", false)]
        public void IsCurrentVersionNewer(string currentVersion, string availableVersion, bool expectedIsCurrentVersionNewer)
        {
            var updateInfo = new UpdateInfo()
            {
                Version = new Version(availableVersion)
            };

            Assert.AreEqual(expectedIsCurrentVersionNewer, updateInfo.IsCurrentVersionNewer(new Version(currentVersion)));
        }
    }
}
