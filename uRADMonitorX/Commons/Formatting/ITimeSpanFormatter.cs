using System;

namespace uRADMonitorX.Commons.Formatting {

    public interface ITimeSpanFormatter {

        String Format(TimeSpan timespan);
    }
}
