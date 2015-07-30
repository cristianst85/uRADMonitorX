namespace uRADMonitorX.Core.Fetchers {

     public interface IDeviceDataFetcher {

        event DeviceDataFetcherEventHandler DeviceDataFetcherEventHandler;

        event DeviceDataFetcherErrorEventHandler DeviceDataFetcherErrorEventHandler;

        bool IsRunning { get; }

        void Start();

        void Stop();

    }
}
