using System;

namespace uRADMonitorX.Core.Jobs
{
    public class FixedIntervalPollingStrategy : IPollingStrategy
    {
        public int PollingInterval { get; private set; }

        public FixedIntervalPollingStrategy(int pollingInterval)
        {
            if (pollingInterval < 1)
            {
                throw new ArgumentException("pollingInterval");
            }

            this.PollingInterval = pollingInterval;
        }

        /// <summary>
        /// Returns the interval when the next polling should occur.
        /// This is always the value of polling interval passed to the
        /// constructor. The current value of the Watchdog Timer is ignored.
        /// </summary>
        /// <param name="wdt">Current value of the Watchdog Timer.</param>
        /// <returns></returns>
        public TimeSpan GetNextInterval(int wdt)
        {
            return TimeSpan.FromSeconds(this.PollingInterval);
        }
    }
}
