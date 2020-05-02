using System;

namespace uRADMonitorX.Core.Jobs
{
    public interface IPollingStrategy
    {
        TimeSpan GetNextInterval(int wdt);
    }
}