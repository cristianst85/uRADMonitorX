using System;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public class NLogWrapperFileAppender : ReconfigurableFileAppender {

        public NLogWrapperFileAppender(String filePath)
            : base(filePath) {
            throw new NotImplementedException();
        }

        public void Write(String message) {
            throw new NotImplementedException();
        }
    }
}
