using HashCheck;
using System;
using System.IO;
using System.Net;
using uRADMonitorX.Updater.GitHubApi;

namespace uRADMonitorX.Updater {

    public class GitHubApplicationUpdater : HttpApplicationUpdater {

        private Release release = null;

        public GitHubApplicationUpdater(String updateUrl)
            : base(updateUrl) {
        }

        public override IApplicationUpdateInfo Check() {
            SetSecurityProtocol();
            String fileContent = this.retrieveContentFromUrl(this.Url);
            this.release = Newtonsoft.Json.JsonConvert.DeserializeObject<Release>(fileContent);
            GitHubApplicationUpdateInfo applicationUpdateInfo = new GitHubApplicationUpdateInfo(this);
            applicationUpdateInfo.Version = release.TagName.TrimStart('v').Trim();
            applicationUpdateInfo.DownloadPage = release.HtmlUrl;
            foreach (var asset in this.release.Assets) {
                if (Path.GetExtension(asset.Name).Equals(".exe", StringComparison.OrdinalIgnoreCase)) {
                    applicationUpdateInfo.DownloadUrl = asset.BrowserDownloadUrl;
                }
            }
            return applicationUpdateInfo;
        }

        public String GetChecksum() {
            SetSecurityProtocol();
            foreach (var asset in this.release.Assets) {
                if (Path.GetExtension(asset.Name).Equals(".md5", StringComparison.OrdinalIgnoreCase)) {
                    String md5FileContent = this.retrieveContentFromUrl(asset.BrowserDownloadUrl);
                    IReadOnlyChecksumFile md5File = ReadOnlyMD5File.Load(md5FileContent);
                    return md5File.Entries[0].Checksum.Value;
                }
            }
            throw new Exception("Checksum for the current release was not found.");
        }

        private void SetSecurityProtocol() {
            // As of February 22, 2018 GitHub API no longer 
            // supports requests made with TLSv1 / TLSv1.1.
            // See: https://githubengineering.com/crypto-removal-notice/
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}
