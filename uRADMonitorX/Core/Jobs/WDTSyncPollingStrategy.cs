﻿using System;

namespace uRADMonitorX.Core.Jobs
{
    public class WDTSyncPollingStrategy : IPollingStrategy
    {
        public const int DefaultWDTInterval = 60;

        public int WDTInterval { get; private set; }

        public WDTSyncPollingStrategy(int wdtInterval)
        {
            if (wdtInterval < 1)
            {
                throw new ArgumentException("wdtInterval");
            }

            this.WDTInterval = wdtInterval;
        }

        /// <summary>
        /// Returns the interval when the next polling should occur.
        /// This is calculated based on the current value of the Watchdog Timer.
        /// </summary>
        /// <param name="wdt">Current value of the Watchdog Timer.</param>
        /// <returns></returns>
        public TimeSpan GetNextInterval(int wdt)
        {
            return TimeSpan.FromSeconds(this.WDTInterval - (wdt % this.WDTInterval) + 1);
        }
    }
}
