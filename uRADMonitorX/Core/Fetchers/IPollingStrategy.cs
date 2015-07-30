
namespace uRADMonitorX.Core.Fetchers {

    public interface IPollingStrategy {

        int GetSecondsUntilNextPoll(int wdt);
    }
}
