using System;
using System.IO;
using System.Net;

namespace uRADMonitorX.Core.Device {

    public class DeviceDataHttpReader : DeviceDataReader, IDeviceDataReader {

        public String IPAddress { get; private set; }
        public int Timeout { get; set; }

        public DeviceDataHttpReader(String ipAddress) {

            if (String.IsNullOrEmpty(ipAddress)) {
                throw new ArgumentNullException("ipAddress");
            }

            if (!uRADMonitorX.Commons.Networking.IPAddress.IsValidFormat(ipAddress) &&
                !uRADMonitorX.Commons.Networking.IPEndPoint.IsValidFormat(ipAddress)) {
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
            request.UserAgent = Program.UserAgent;
            if (this.Timeout > 0) {
                request.Timeout = this.Timeout; // TODO: HttpWebRequest seems to have a default timeout of approx. 20 sec.
            }
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