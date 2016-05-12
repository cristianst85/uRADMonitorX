using System;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public interface ILoggerAppender {

        void Append(String message);

    }
}
