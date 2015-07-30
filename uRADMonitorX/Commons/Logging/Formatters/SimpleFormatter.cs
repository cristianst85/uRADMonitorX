using System;

namespace uRADMonitorX.Commons.Logging.Formatters {

    public class SimpleFormatter : ILoggerFormatter {

        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        private String dateTimeFormat = null;

        public SimpleFormatter() {
            this.dateTimeFormat = DefaultDateTimeFormat;
        }

        public String Format(String message) {
            return String.Format("[{0}] {1}", DateTime.Now.ToString(dateTimeFormat), message);
        }
    }
}
