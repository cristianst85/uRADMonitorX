using System;
using System.IO;
using HashCheck;
using uRADMonitorX.Updater.GitHubApi;

namespace uRADMonitorX.Updater {

    public class GitHubApplicationUpdater : HttpApplicationUpdater {

        public GitHubApplicationUpdater(String updateUrl)
            : base(updateUrl) {
        }

        public override ApplicationUpdateInfo Check() {
            String fileContent = this.retrieveContentFromUrl(this.Url);
            Release release = Newtonsoft.Json.JsonConvert.DeserializeObject<Release>(fileContent);
            ApplicationUpdateInfo applicationUpdateInfo = new ApplicationUpdateInfo();
            applicationUpdateInfo.Version = release.TagName.TrimStart('v').Trim();
            applicationUpdateInfo.DownloadPage = release.HtmlUrl;
            foreach (ReleaseAsset asset in release.Assets) {
                if (Path.GetExtension(asset.Name).Equals(".md5", StringComparison.OrdinalIgnoreCase)) {
                    String md5FileContent = this.retrieveContentFromUrl(asset.BrowserDownloadUrl);
                    IReadOnlyChecksumFile md5File = ReadOnlyMD5File.Load(md5FileContent);
                    applicationUpdateInfo.FileHash = md5File.Entries[0].Checksum.Value;
                }
                else if (Path.GetExtension(asset.Name).Equals(".exe", StringComparison.OrdinalIgnoreCase)) {
                    applicationUpdateInfo.DownloadUrl = asset.BrowserDownloadUrl;
                }
            }
            return applicationUpdateInfo;
        }
    }
}
