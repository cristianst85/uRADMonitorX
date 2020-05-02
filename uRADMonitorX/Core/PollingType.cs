namespace uRADMonitorX.Core
{
    public enum PollingType
    {
        /// <summary>
        /// A fixed interval specified by the user.
        /// </summary>
        FixedInterval,

        /// <summary>
        /// Synchronize with the Watchdog Timer.
        /// </summary>
        WDTSync
    }
}
