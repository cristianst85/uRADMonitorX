using System;

namespace uRADMonitorX.Core.Jobs
{
    public class DeviceDataJobErrorEventArgs
    {
        public Exception Exception { get; private set; }

        public DeviceDataJobErrorEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
