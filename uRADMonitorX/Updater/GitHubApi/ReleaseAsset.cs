using System;
using Newtonsoft.Json;

namespace uRADMonitorX.Updater.GitHubApi {

    public class ReleaseAsset {

        [JsonPropertyAttribute(PropertyName = "name")]
        public String Name { get; set; }
        [JsonPropertyAttribute(PropertyName = "browser_download_url")]
        public String BrowserDownloadUrl { get; set; }
    }
}
