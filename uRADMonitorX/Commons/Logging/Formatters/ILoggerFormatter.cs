using System;

namespace uRADMonitorX.Commons.Logging.Formatters {

    public interface ILoggerFormatter {

        String Format(String message);
    }
}
