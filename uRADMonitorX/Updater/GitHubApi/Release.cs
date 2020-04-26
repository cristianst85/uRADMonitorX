using Newtonsoft.Json;
using System.Collections.Generic;

namespace uRADMonitorX.Updater.GitHubApi
{
    public class Release
    {
        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "tag_name")]
        public string TagName { get; set; }

        [JsonProperty(PropertyName = "assets")]
        public ICollection<ReleaseAsset> Assets { get; set; }
    }
}
