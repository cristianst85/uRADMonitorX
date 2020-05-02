using System;

namespace uRADMonitorX.Core.Jobs
{
    public interface IDeviceDataJob : IDisposable
    {
        event DeviceDataJobEventHandler DeviceDataJobEventHandler;

        event DeviceDataJobErrorEventHandler DeviceDataJobErrorEventHandler;

        bool IsRunning { get; }

        void Start();

        void Stop();
    }
}
