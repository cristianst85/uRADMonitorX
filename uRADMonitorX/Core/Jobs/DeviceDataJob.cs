using System;
using System.Diagnostics;
using System.Timers;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Core.Jobs
{
    public delegate void DeviceDataJobEventHandler(object sender, DeviceDataJobEventArgs e);

    public delegate void DeviceDataJobErrorEventHandler(object sender, DeviceDataJobErrorEventArgs e);

    public class DeviceDataJob : IDeviceDataJob, IDisposable
    {
        /// <summary>
        /// The default polling interval (10 seconds) on which the next 
        /// polling should occur after <c>deviceDataReader</c> fails. 
        /// </summary>
        public readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(10);

        private readonly IDeviceDataReader deviceDataReader;
        private readonly IPollingStrategy pollingStrategy;

        private Timer timer;

        private volatile bool isRunning;
        private volatile bool isStopped;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public event DeviceDataJobEventHandler DeviceDataJobEventHandler;

        public event DeviceDataJobErrorEventHandler DeviceDataJobErrorEventHandler;

        public DeviceDataJob(IDeviceDataReader deviceDataReader, IPollingStrategy pollingStrategy)
        {
            this.deviceDataReader = deviceDataReader;
            this.pollingStrategy = pollingStrategy;
        }

        public void Start()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] Start()");

            this.isStopped = false;

            if (this.timer.IsNull())
            {
                this.timer = new Timer();
                this.timer.Elapsed += new ElapsedEventHandler(Run);
            }

            // When the job is started, start the timer immediately.
            this.timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            this.timer.Start();
        }

        public void Stop()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] Stop()");

            this.isStopped = true;

            if (this.timer.IsNotNull())
            {
                this.timer.Stop();
            }
        }

        private void Run(object o, EventArgs e)
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] Run()");

            if (this.isRunning)
            {
                return;
            }

            this.isRunning = true;

            try
            {
                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] Timer Interval is {this.timer.Interval } ms");
                this.timer.Stop();

                // Set timer interval to the default polling interval in case that deviceDataReader fails.
                this.timer.Interval = DefaultPollingInterval.TotalMilliseconds;

                // Read device data.
                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] DeviceDataReader.Read()");
                var deviceData = this.deviceDataReader.Read();

                // Update the timer interval according to the polling strategy.
                this.timer.Interval = this.pollingStrategy.GetNextInterval(deviceData.WDT).TotalMilliseconds;
                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] Timer Interval was set to {this.timer.Interval } ms");

                this.OnSuccess(deviceData);
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
            finally
            {
                this.isRunning = false;

                if (!this.isStopped)
                {
                    this.timer.Start();
                }
            }
        }

        protected virtual void OnSuccess(DeviceData deviceData)
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] OnSuccess()");

            DeviceDataJobEventHandler?.Invoke(this, new DeviceDataJobEventArgs(deviceData));
        }

        protected virtual void OnError(Exception exception)
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] OnError()");

            DeviceDataJobErrorEventHandler?.Invoke(this, new DeviceDataJobErrorEventArgs(exception));
        }

        private bool disposed;

        public void Dispose()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(DeviceDataJob)}] Dispose()");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.timer != null)
                    {
                        this.timer.Stop();
                        this.timer.Elapsed -= Run;
                        this.timer.Dispose();

                        this.timer = null;
                    }

                    this.disposed = true;
                }
            }
        }
    }
}
