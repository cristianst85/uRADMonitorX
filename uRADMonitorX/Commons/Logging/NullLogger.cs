using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX.Commons.Logging
{
    public class NullLogger : ILogger
    {
        public ILoggerAppender Appender { get; private set; }

        public ILoggerFormatter Formatter { get; private set; }

        public bool Enabled { get; set; }

        public NullLogger()
        {
            this.Appender = new NullAppender();
            this.Formatter = new PassthroughFormatter();
        }

        public void Write(string message)
        {
            ; // Do nothing. All messages are discarded.
        }
    }
}
