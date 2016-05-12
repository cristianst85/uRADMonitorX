using System;

namespace uRADMonitorX.Updater {

    public interface IHttpApplicationUpdater {

        IApplicationUpdateInfo Check();

        void Download(String downloadUrl, String filePath);

    }
}
