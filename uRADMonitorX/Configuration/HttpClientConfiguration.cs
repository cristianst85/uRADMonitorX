using uRADMonitorX.Commons;

namespace uRADMonitorX.Configuration
{
    public class HttpClientConfiguration : IHttpClientConfiguration
    {
        public string UserAgent { get; set; }

        public HttpClientConfiguration()
        {
            this.UserAgent = Program.Settings.UserAgent;
        }

        public virtual string GetUserAgent()
        {
            return this.UserAgent;
        }
    }
}
