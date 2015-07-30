using System;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public interface ILoggerAppender {

        bool Enabled { get; set; }

        void Append(String message);

    }
}
