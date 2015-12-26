using System;

namespace uRADMonitorX.Updater {

    public interface IHttpApplicationUpdater {

        ApplicationUpdateInfo Check();

        void Download(String downloadUrl, String filePath);

    }
}
