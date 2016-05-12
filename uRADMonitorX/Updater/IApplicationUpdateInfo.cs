using System;

namespace uRADMonitorX.Updater {

    public interface IApplicationUpdateInfo {

        string DownloadPage { get; set; }
        string DownloadUrl { get; set; }
        string FileHash { get; set; }
        bool IsNewVersionAvailable(Version currentVersion);
        string Version { get; set; }
    }
}
