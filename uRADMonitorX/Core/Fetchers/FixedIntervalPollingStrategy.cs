using System;

namespace uRADMonitorX.Core.Fetchers {

    public class FixedIntervalPollingStrategy : IPollingStrategy {

        public int PollingInterval { get; private set; }

        public FixedIntervalPollingStrategy(int pollingInterval) {

            if (pollingInterval < 1) {
                throw new ArgumentException("pollingInterval");
            }
            this.PollingInterval = pollingInterval;
        }

        /// <summary>
        /// Returns the number of seconds until next polling that 
        /// is always the value of polling interval specified 
        /// in the constructor. The value of WDT is ignored.
        /// </summary>
        /// <param name="wdt"></param>
        /// <returns></returns>
        public int GetSecondsUntilNextPoll(int wdt) {
            return this.PollingInterval;
        }
    }
}
