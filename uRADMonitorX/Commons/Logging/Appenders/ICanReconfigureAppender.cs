using System;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public interface ICanReconfigureAppender {

        void Reconfigure(String filePath);

    }
}
