using System;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public interface IReconfigurableFileAppender {

        void Reconfigure(String newFilePath);

    }
}
