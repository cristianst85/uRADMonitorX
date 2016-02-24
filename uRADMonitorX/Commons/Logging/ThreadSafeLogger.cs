using System;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX.Commons.Logging {

    public class ThreadSafeLogger : ILogger {

        private object _locker = new object();

        private ILogger logger;

        public ThreadSafeLogger(ILogger logger) {
            this.logger = logger;
        }

        public void Write(String message) {
            lock (_locker) {
                this.logger.Write(message);
            }
        }

        public ILoggerAppender Appender {
            get {
                return this.logger.Appender;
            }
        }

        public ILoggerFormatter Formatter {
            get {
                return this.logger.Formatter;
            }
        }

        public bool Enabled {
            get {
                return this.logger.Enabled;
            }
            set {
                this.logger.Enabled = value;
            }
        }
    }
}
