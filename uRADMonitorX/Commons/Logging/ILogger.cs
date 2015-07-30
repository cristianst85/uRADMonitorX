using System;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX.Commons.Logging {

    public interface ILogger {

        ILoggerAppender Appender { get; }

        ILoggerFormatter Formatter { get; }

        bool Enabled { get; set; }

        void Write(String message);
    }
}
