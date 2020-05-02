using uRADMonitorX.Core.Jobs;

namespace uRADMonitorX.Core
{
    public interface IDeviceDataJobFactory
    {
        IDeviceDataJob Create();
    }
}
