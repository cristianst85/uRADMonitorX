using System;
using System.Timers;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Core.Fetchers {

    public class DeviceDataFetcher : IDeviceDataFetcher {

        private IDeviceDataReader deviceDataReader;

        private volatile bool _isRunning;
        public bool IsRunning {
            get {
                return this._isRunning;
            }
            private set {
                this._isRunning = value;
            }
        }

        private IPollingStrategy pollingStrategy;
        private Timer timer;
        private volatile bool stopped;

        private int _defaultPollingInterval = 5;
        /// <summary>
        /// Sets the default polling interval in seconds on which 
        /// next polling occurs after <c>deviceDataReader</c> fails.
        /// <para> </para>
        /// <para>By default, this value is set to 5 seconds.</para>
        /// <para> </para>
        /// <para>Throws <c>System.ArgumentException</c> if the default 
        /// polling interval is less than one second.</para>
        /// </summary>
        public int DefaultPollingInterval {
            get {
                return _defaultPollingInterval;
            }
            private set {
                if (value < 1) {
                    throw new ArgumentException();
                }
                this._defaultPollingInterval = value;
            }
        }


        public event DeviceDataFetcherEventHandler DeviceDataFetcherEventHandler;
        public event DeviceDataFetcherErrorEventHandler DeviceDataFetcherErrorEventHandler;

        public DeviceDataFetcher(IDeviceDataReader deviceDataReader, IPollingStrategy pollingStrategy) {
            this.deviceDataReader = deviceDataReader;
            this.pollingStrategy = pollingStrategy;
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
                // Set timer interval to the default polling interval in case that deviceDataReader fails.
                this.timer.Interval = this.DefaultPollingInterval;
                DeviceData deviceData = this.deviceDataReader.Read();
                // Update timer interval according with polling strategy.
                this.timer.Interval = (double)(this.pollingStrategy.GetSecondsUntilNextPoll(deviceData.WDT) * 1000);
                this.OnSuccess(deviceData);
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
