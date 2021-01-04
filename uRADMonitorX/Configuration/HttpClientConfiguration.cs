using uRADMonitorX.Commons;

namespace uRADMonitorX.Configuration
{
    public class HttpClientConfiguration : IHttpClientConfiguration
    {
        public string GetUserAgent()
        {
            return Program.Settings.UserAgent;
        }
    }
}
