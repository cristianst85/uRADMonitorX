using System;
using System.IO;
using System.Net;

namespace uRADMonitorX.Updater {

    public class HttpApplicationUpdater : IHttpApplicationUpdater {

        public String Url { get; private set; }

        public HttpApplicationUpdater(String updateUrl) {
            if (updateUrl == null) {
                throw new ArgumentNullException("updateUrl");
            }
            if (updateUrl.Length == 0) {
                throw new ArgumentException("Update URL cannot be an empty string.");
            }
            if (!updateUrl.StartsWith("https://")) {
                throw new ArgumentException("A non-secure update URL is not permitted.");
            }
            this.Url = updateUrl;
        }

        public virtual ApplicationUpdateInfo Check() {
            String fileContent = this.retrieveContentFromUrl(this.Url);
            return ApplicationUpdateInfo.LoadFromXml(fileContent);
        }

        public void Download(String downloadUrl, String filePath) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(downloadUrl);
            request.UserAgent = Program.UserAgent;
            byte[] fileContent;

            using (MemoryStream ms = new MemoryStream()) {
                using (WebResponse response = request.GetResponse()) {
                    Stream data = response.GetResponseStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    do {
                        bytesRead = data.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, bytesRead);

                    } while (bytesRead > 0);
                }
                fileContent = ms.ToArray();
            }
            File.WriteAllBytes(filePath, fileContent);
        }

        protected virtual String retrieveContentFromUrl(String url) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = Program.UserAgent;
            String content = String.Empty;
            using (WebResponse response = request.GetResponse()) {
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data)) {
                    content = sr.ReadToEnd();
                }
            }
            return content;
        }
    }
}
