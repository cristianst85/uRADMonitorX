using System;

namespace uRADMonitorX.Commons.Logging.Appenders
{
    public interface IReconfigurableFileAppender
    {
        void Reconfigure(string newFilePath);
    }
}
