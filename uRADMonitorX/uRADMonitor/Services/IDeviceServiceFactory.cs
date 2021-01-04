namespace uRADMonitorX.uRADMonitor.Services
{
    public interface IDeviceServiceFactory
    {
        IDeviceService Create(string userId, string userKey);
    }
}
