using System.IO;
using System.Net;

namespace uRADMonitorX.Commons
{
    public class HttpClient : IHttpClient
    {
        public string UserAgent { get; private set; }

        public HttpClient(string userAgent)
        {
            this.UserAgent = userAgent;
        }

        public virtual string Get(string url)
        {
            return GetContent(url, UserAgent);
        }

        protected virtual string GetContent(string url, string userAgent = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.UserAgent = userAgent ?? Program.UserAgent;

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
