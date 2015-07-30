using System;
using System.Timers;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Core.Fetchers {

    public delegate void DeviceDataFetcherEventHandler(object sender, DeviceDataFetcherEventArgs e);

    public delegate void DeviceDataFetcherErrorEventHandler(object sender, DeviceDataFetcherErrorEventArgs e);

    public class FixedIntervalDeviceDataFetcher : IDeviceDataFetcher {

        private IDeviceDataReader deviceDataReader;

        private volatile bool _isRunning;
        public bool IsRunning {
            get {
                return this._isRunning;
            }
            set {
                this._isRunning = value;
            }
        }

        private Timer timer;
        private int pollingInterval;
        private volatile bool stopped;

        public event DeviceDataFetcherEventHandler DeviceDataFetcherEventHandler;
        public event DeviceDataFetcherErrorEventHandler DeviceDataFetcherErrorEventHandler;

        public FixedIntervalDeviceDataFetcher(IDeviceDataReader deviceDataReader, int pollingInterval) {
            this.deviceDataReader = deviceDataReader;
            this.pollingInterval = pollingInterval;
        }

        public void Start() {
            this.stopped = false;
            if (this.timer == null) {
                this.timer = new System.Timers.Timer();
                this.timer.Interval = 1000; // Execute immediately.
                this.timer.Enabled = true;
                this.timer.Elapsed += new ElapsedEventHandler(doWork);
                this.timer.Start();
            }
            else {
                this.timer.Interval = 1000; // Execute immediately.
                this.timer.Start();
            }
        }

        public void Stop() {
            this.stopped = true;
            if (this.timer != null) {
                this.timer.Stop();
            }
        }

        private void doWork(object o, EventArgs e) {
            this.IsRunning = true;
            try {
                this.timer.Stop();
                this.timer.Interval = this.pollingInterval * 1000; // Set interval to the polling interval value.
                this.OnSuccess(this.deviceDataReader.Read());
            }
            catch (Exception ex) {
                this.OnError(ex);
            }
            finally {
                this.IsRunning = false;
                if (!this.stopped) {
                    this.timer.Start();
                }
            }
        }

        protected virtual void OnSuccess(DeviceData deviceData) {
            DeviceDataFetcherEventHandler handler = DeviceDataFetcherEventHandler;
            if (handler != null) {
                handler(this, new DeviceDataFetcherEventArgs(deviceData));
            }
        }

        protected virtual void OnError(Exception exception) {
            DeviceDataFetcherErrorEventHandler handler = DeviceDataFetcherErrorEventHandler;
            if (handler != null) {
                handler(this, new DeviceDataFetcherErrorEventArgs(exception));
            }
        }
    }
}
