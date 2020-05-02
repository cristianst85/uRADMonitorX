using System;

namespace uRADMonitorX.Commons.Logging.Formatters
{
    public class PassthroughFormatter : ILoggerFormatter
    {
        public string Format(string message)
        {
            return message;
        }
    }
}