namespace uRADMonitorX.Core.Jobs
{
    public interface IPollingStrategy
    {
        int GetNextInterval(int wdt);
    }
}