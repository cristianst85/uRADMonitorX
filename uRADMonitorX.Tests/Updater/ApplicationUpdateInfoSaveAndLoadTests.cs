using System;
using System.IO;
using NUnit.Framework;
using uRADMonitorX.Updater;

namespace uRADMonitorX.Tests.Updater {

    [TestFixture]
    public class ApplicationUpdateInfoSaveAndLoadTests {

        private String outputfilePath = Path.GetFullPath(@"..\..\..\uRADMonitorX.Tests.Files\updater\update.xml");

        [SetUp]
        public void SetUp() {
            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [TearDown]
        public void TearDown() {
            Assert.IsTrue(File.Exists(outputfilePath));
            File.Delete(outputfilePath);
            Assert.IsFalse(File.Exists(outputfilePath));
        }

        [Test]
        public void SaveAndLoadFromXmlFile() {
            ApplicationUpdateInfo applicationUpdateInfo = new ApplicationUpdateInfo() {
                Version = "0.1.2",
                DownloadUrl = "https://mydomain.com/uRADMonitor.exe",
                DownloadPage = "https://mydomain.com/Update.html",
                FileHash = "d41d8cd98f00b204e9800998ecf8427e"
            };
            applicationUpdateInfo.Save(outputfilePath);
            Assert.IsTrue(File.Exists(outputfilePath));
            applicationUpdateInfo = ApplicationUpdateInfo.LoadFromXmlFile(outputfilePath);
            Assert.AreEqual("0.1.2", applicationUpdateInfo.Version);
            Assert.AreEqual("https://mydomain.com/uRADMonitor.exe", applicationUpdateInfo.DownloadUrl);
            Assert.AreEqual("https://mydomain.com/Update.html", applicationUpdateInfo.DownloadPage);
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", applicationUpdateInfo.FileHash);
        }

        [Test]
        public void SaveAndLoadFromXml() {
            ApplicationUpdateInfo applicationUpdateInfo = new ApplicationUpdateInfo() {
                Version = "0.1.2",
                DownloadUrl = "https://mydomain.com/uRADMonitor.exe",
                DownloadPage = "https://mydomain.com/Update.html",
                FileHash = "d41d8cd98f00b204e9800998ecf8427e"
            };
            applicationUpdateInfo.Save(outputfilePath);
            Assert.IsTrue(File.Exists(outputfilePath));
            applicationUpdateInfo = ApplicationUpdateInfo.LoadFromXml(File.ReadAllText(outputfilePath));
            Assert.AreEqual("0.1.2", applicationUpdateInfo.Version);
            Assert.AreEqual("https://mydomain.com/uRADMonitor.exe", applicationUpdateInfo.DownloadUrl);
            Assert.AreEqual("https://mydomain.com/Update.html", applicationUpdateInfo.DownloadPage);
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", applicationUpdateInfo.FileHash);
        }
    }
}
