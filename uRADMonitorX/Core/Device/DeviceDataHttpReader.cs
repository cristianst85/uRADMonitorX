using System;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Core.Device
{
    public class DeviceDataHttpReader : DeviceDataReader
    {
        public string DeviceUrl { get; private set; }

        public IHttpClient HttpClient { get; private set; }

        public DeviceDataHttpReader(IHttpClient httpClient, string ipAddress)
        {
            if (ipAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException("ipAddress");
            }

            if (!Commons.Networking.IPAddress.IsValidFormat(ipAddress) && !IPEndPoint.IsValidFormat(ipAddress))
            {
                throw new ArgumentException("Invalid IP address.");
            }

            this.HttpClient = httpClient;
            this.DeviceUrl = string.Format("http://{0}/", ipAddress);
        }

        public override DeviceData Read()
        {
            string htmlContent = this.HttpClient.Get(this.DeviceUrl);

            return this.Parse(htmlContent);
        }
    }
}
