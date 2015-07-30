using System;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX.Commons.Logging {

    public class ThreadSafeLogger : Logger {

        private object _locker = new object();

        public ThreadSafeLogger(ILoggerAppender appender, ILoggerFormatter formatter)
            : base(appender, formatter) {
        }

        public override void Write(String message) {
            lock (_locker) {
                base.Write(message);
            }
        }
    }
}
