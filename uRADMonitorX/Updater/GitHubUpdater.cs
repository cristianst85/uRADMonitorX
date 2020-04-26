using HashCheck;
using Newtonsoft.Json;
using System;
using System.IO;
using uRADMonitorX.Helpers;
using uRADMonitorX.Updater.GitHubApi;

namespace uRADMonitorX.Updater
{
    public class GitHubUpdater : WebUpdater
    {
        public GitHubUpdater()
        {
        }

        public override IUpdateInfo Check(string updateUrl)
        {
            SetSecurityProtocol();

            return base.Check(updateUrl);
        }

        protected override IUpdateInfo InternalCheck(string fileContent)
        {
            var release = JsonConvert.DeserializeObject<Release>(fileContent);

            var updateInfo = new UpdateInfo()
            {
                Version = new Version(release.TagName.TrimStart('v').Trim()),
                DownloadPage = release.HtmlUrl
            };

            foreach (var asset in release.Assets)
            {
                if (Path.GetExtension(asset.Name).Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    updateInfo.DownloadUrl = asset.BrowserDownloadUrl;
                }
                else if (Path.GetExtension(asset.Name).Equals(".md5", StringComparison.OrdinalIgnoreCase))
                {
                    var md5FileContent = this.RetrieveContentFromUrl(asset.BrowserDownloadUrl);
                    var md5File = ReadOnlyMD5File.Load(md5FileContent);

                    updateInfo.Checksum = md5File.Entries[0].Checksum;
                }
            }

            if (updateInfo.Checksum == null)
            {
                throw new Exception("Checksum file was not found.");
            }

            return updateInfo;
        }

        private void SetSecurityProtocol()
        {
            // As of February 22, 2018 GitHub API no longer supports TLSv1 / TLSv1.1 requests.
            // See: https://githubengineering.com/crypto-removal-notice/
            ServicePointManagerHelper.SetSecurityProtocolToTls12();
        }
    }
}
