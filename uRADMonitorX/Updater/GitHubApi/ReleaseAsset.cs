using Newtonsoft.Json;

namespace uRADMonitorX.Updater.GitHubApi
{
    public class ReleaseAsset
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}
