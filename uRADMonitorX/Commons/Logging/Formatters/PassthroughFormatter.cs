using System;

namespace uRADMonitorX.Commons.Logging.Formatters {

    public class PassthroughFormatter : ILoggerFormatter {

        public String Format(String message) {
            return message;
        }
    }
}
