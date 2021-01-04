namespace uRADMonitorX.uRADMonitor.Configuration
{
    public interface IDeviceDataClientConfiguration
    {
        string GetEndpointUrl();

        string GetUserAgent();
    }
}
