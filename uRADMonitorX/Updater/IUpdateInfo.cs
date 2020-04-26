using HashCheck;
using System;

namespace uRADMonitorX.Updater
{
    public interface IUpdateInfo
    {
        Version Version { get; set; }

        string DownloadPage { get; set; }

        string DownloadUrl { get; set; }

        IChecksum Checksum { get; set; }

        bool IsNewVersionAvailable(Version currentVersion);

        bool IsCurrentVersionNewer(Version currentVersion);
    }
}
