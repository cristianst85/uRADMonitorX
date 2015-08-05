using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Core {

    public interface IDeviceDataReaderFactory {

        IDeviceDataReader Create();
    }
}
