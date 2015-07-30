using System;

namespace uRADMonitorX.Core.Fetchers {

    public class DeviceDataFetcherErrorEventArgs {

        public Exception Exception { get; private set; }

        public DeviceDataFetcherErrorEventArgs(Exception exception) {
            this.Exception = exception;
        }
    }
}
