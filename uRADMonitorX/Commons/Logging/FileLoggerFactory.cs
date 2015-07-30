using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX.Commons.Logging {

    public class FileLoggerFactory : ILoggerFactory {

        private ILoggerAppender appender;
        private ILoggerFormatter formatter;

        public FileLoggerFactory(ILoggerAppender appender, ILoggerFormatter formatter) {
            this.appender = appender;
            this.formatter = formatter;
        }

        public ILogger Create() {
            return new Logger(appender, formatter);
        }
    }
}
