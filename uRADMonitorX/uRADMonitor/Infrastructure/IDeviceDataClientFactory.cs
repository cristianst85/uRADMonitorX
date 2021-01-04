namespace uRADMonitorX.uRADMonitor.Infrastructure
{
    public interface IDeviceDataClientFactory
    {
        IDeviceDataClient Create(string userId, string userKey);
    }
}
