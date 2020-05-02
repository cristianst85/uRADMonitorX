using System;

namespace uRADMonitorX.Commons.Formatting
{
    public interface ITimeSpanFormatter
    {
        string Format(TimeSpan timespan);
    }
}
