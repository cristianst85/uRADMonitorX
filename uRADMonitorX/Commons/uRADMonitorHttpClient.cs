using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using uRADMonitorX.Helpers;

namespace uRADMonitorX.Commons
{
    [SuppressMessage(category: "Style", checkId: "IDE1006")]
    public class uRADMonitorHttpClient : IHttpClient
    {
        private readonly string userAgent;
        private readonly string userId;
        private readonly string userKey;

        public uRADMonitorHttpClient(string userAgent, string userId, string userKey)
        {
            this.userAgent = userAgent;
            this.userId = userId;
            this.userKey = userKey;
        }

        public virtual string Get(string url)
        {
            ServicePointManagerHelper.SetSecurityProtocolToTls12();

            return GetContent(url);
        }

        protected virtual string GetContent(string url, string userAgent = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.UserAgent = this.userAgent ?? Program.Settings.UserAgent;

            if (userId != null)
            {
                request.Headers.Add($"X-User-id:{this.userId}");
            }

            if (userKey != null)
            {
                request.Headers.Add($"X-User-hash:{this.userKey}");
            }

            using (var response = request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
