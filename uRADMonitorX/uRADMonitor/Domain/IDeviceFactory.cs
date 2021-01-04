using uRADMonitorX.uRADMonitor.API.V1;

namespace uRADMonitorX.uRADMonitor.Domain
{
    public interface IDeviceFactory
    {
        Device Create(DeviceDto deviceDto);
    }
}
