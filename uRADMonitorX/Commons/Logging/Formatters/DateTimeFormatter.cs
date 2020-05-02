using System;

namespace uRADMonitorX.Commons.Logging.Formatters
{
    public class DateTimeFormatter : ILoggerFormatter
    {
        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        private readonly string dateTimeFormat = null;

        public DateTimeFormatter()
        {
            this.dateTimeFormat = DefaultDateTimeFormat;
        }

        public string Format(string message)
        {
            return string.Format("[{0}] {1}", DateTime.Now.ToString(dateTimeFormat), message);
        }
    }
}
