using System;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public class NullAppender : ILoggerAppender {

        public NullAppender() {
        }

        public void Append(String message) {
            ; // Do nothing. All messages are discarded.
        }
    }
}
