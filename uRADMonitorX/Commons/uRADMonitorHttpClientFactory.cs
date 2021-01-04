using System.Diagnostics.CodeAnalysis;

namespace uRADMonitorX.Commons
{
    [SuppressMessage(category: "Style", checkId: "IDE1006")]
    public class uRADMonitorHttpClientFactory : IHttpClientFactory
    {
        private readonly IHttpClientConfiguration httpClientConfiguration;

        public uRADMonitorHttpClientFactory(IHttpClientConfiguration httpClientConfiguration)
        {
            this.httpClientConfiguration = httpClientConfiguration;
        }

        public IHttpClient Create(string userId, string userKey)
        {
            return new uRADMonitorHttpClient(httpClientConfiguration.GetUserAgent(), userId, userKey);
        }
    }
}
