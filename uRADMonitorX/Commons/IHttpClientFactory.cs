namespace uRADMonitorX.Commons
{
    public interface IHttpClientFactory
    {
        IHttpClient Create(string userId, string userKey);
    }
}
