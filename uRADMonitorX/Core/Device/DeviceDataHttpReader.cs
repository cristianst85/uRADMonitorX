using System;
using System.IO;
using System.Net;
using uRADMonitorX.Commons.Networking;

namespace uRADMonitorX.Core.Device {

    public class DeviceDataHttpReader : DeviceDataReader, IDeviceDataReader {

        public String IPAddress { get; private set; }

        public DeviceDataHttpReader(String ipAddress) {

            if (String.IsNullOrEmpty(ipAddress)) {
                throw new ArgumentNullException("ipAddress");
            }

            if (!uRADMonitorX.Commons.Networking.IPAddress.IsValidFormat(ipAddress)) {
                throw new ArgumentException("Invalid IP address.");
            }

            this.IPAddress = ipAddress;
        }

        public DeviceData Read() {
            String htmlContent = this.retrieveContentFromUrl(String.Format("http://{0}/", this.IPAddress));
            return this.internalParse(htmlContent);
        }

        private String retrieveContentFromUrl(String url) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "uRADMonitorX/1.0"; // TODO: move this to another class (Application.Constants)
            String htmlContent = String.Empty;
            using (WebResponse response = request.GetResponse()) {
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data)) {
                    htmlContent = sr.ReadToEnd();
                }
            }
            return htmlContent;
        }
    }
}