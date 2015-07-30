using System;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX.Commons.Logging {

    public class Logger : ILogger {

        private volatile bool _enabled;
        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
            }
        }

        public ILoggerAppender Appender { get; private set; }
        public ILoggerFormatter Formatter { get; private set; }

        public Logger(ILoggerAppender appender, ILoggerFormatter formatter) {
            this.Appender = appender;
            this.Formatter = formatter;
        }

        public virtual void Write(String message) {
            if (this.Enabled) {
                this.Appender.Append(this.Formatter.Format(message));
            }
        }
    }
}
