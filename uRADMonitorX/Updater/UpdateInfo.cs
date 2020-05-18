using HashCheck;
using System;

namespace uRADMonitorX.Updater
{
    public class UpdateInfo : IUpdateInfo
    {
        public Version Version { get; set; }

        public string DownloadPage { get; set; }

        public string DownloadUrl { get; set; }

        public string ChecksumUrl { get; set; }

        public UpdateInfo()
        {
        }

        public bool IsNewVersionAvailable(Version currentVersion)
        {
            return Version.CompareTo(GetVersionWithoutRevision(currentVersion)) == 1;
        }

        public bool IsCurrentVersionNewer(Version currentVersion)
        {
            return Version.CompareTo(GetVersionWithoutRevision(currentVersion)) == -1;
        }

        private Version GetVersionWithoutRevision(Version version)
        {
            return new Version(version.Major, version.Minor, version.Build);
        }
    }
}