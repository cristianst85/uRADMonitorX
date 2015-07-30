using System;

namespace uRADMonitorX.Core.Fetchers {

    public class WDTSyncPollingStrategy : IPollingStrategy {

        public int WDTPeriod { get; private set; }

        public WDTSyncPollingStrategy(int wdtPeriod) {

            if (wdtPeriod < 1) {
                throw new ArgumentException("wdtPeriod");
            }

            this.WDTPeriod = wdtPeriod;
        }

        /// <summary>
        /// Returns the number of seconds until next polling 
        /// calculated from the current value of WDT.
        /// </summary>
        /// <param name="wdt"></param>
        /// <returns></returns>
        public int GetSecondsUntilNextPoll(int wdt) {
            return (this.WDTPeriod - (wdt % this.WDTPeriod) + 1);
        }
    }
}
