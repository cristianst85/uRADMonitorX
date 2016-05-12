using System;
using System.Xml;

namespace uRADMonitorX.Updater {

    public class ApplicationUpdateInfo : IApplicationUpdateInfo {

        public String Version { get; set; }
        public String DownloadUrl { get; set; }
        public String DownloadPage { get; set; }
        public String FileHash { get; set; }

        public ApplicationUpdateInfo() {
        }

        public bool IsNewVersionAvailable(Version currentVersion) {
            Version availableVersion = new Version(this.Version);
            return (availableVersion.CompareTo(currentVersion) == 1);
        }

        public void Save(String filePath) {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() {
                Indent = true,
                IndentChars = "  "
            };
            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings)) {
                xmlWriter.WriteStartElement("update");
                writeFullElement(xmlWriter, "version", this.Version);
                writeFullElement(xmlWriter, "downloadUrl", this.DownloadUrl);
                writeFullElement(xmlWriter, "downloadPage", this.DownloadPage);
                writeFullElement(xmlWriter, "fileHash", this.FileHash);
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        private static void writeFullElement(XmlWriter xmlWriter, String name, object value) {
            xmlWriter.WriteStartElement(name);
            if (value == null) {
                xmlWriter.WriteValue(String.Empty);
            }
            else {
                xmlWriter.WriteValue(value);
            }
            xmlWriter.WriteEndElement();
        }

        public static ApplicationUpdateInfo LoadFromXmlFile(String filePath) {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            return internalLoad(xmlDocument);
        }

        public static ApplicationUpdateInfo LoadFromXml(String content) {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            return internalLoad(xmlDocument);
        }

        private static ApplicationUpdateInfo internalLoad(XmlDocument xmlDocument) {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("/update");
            ApplicationUpdateInfo applicationUpdateInfo = new ApplicationUpdateInfo();
            applicationUpdateInfo.Version = xmlNode.SelectSingleNode("version").InnerText;
            applicationUpdateInfo.DownloadUrl = xmlNode.SelectSingleNode("downloadUrl").InnerText;
            applicationUpdateInfo.DownloadPage = xmlNode.SelectSingleNode("downloadPage").InnerText;
            applicationUpdateInfo.FileHash = xmlNode.SelectSingleNode("fileHash").InnerText;
            return applicationUpdateInfo;
        }
    }
}
