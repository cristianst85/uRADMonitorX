using System;

namespace uRADMonitorX.Commons.Formatting
{
    public class TimeSpanFormatter : ITimeSpanFormatter
    {
        public TimeSpanFormatter()
        {
        }

        public string Format(TimeSpan timespan)
        {
            if (timespan.TotalSeconds < 60)
            {
                return string.Format("{0}s", timespan.Seconds);
            }
            else if (timespan.TotalSeconds < 3600)
            {
                return string.Format("{0}m {1}s", timespan.Minutes, timespan.Seconds);
            }
            else if (timespan.TotalSeconds < 86400)
            {
                return string.Format("{0}h {1}m {2}s", timespan.Hours, timespan.Minutes, timespan.Seconds);
            }
            else
            {
                return string.Format("{0}d {1}h {2}m {3}s", timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds);
            }
        }
    }
}
