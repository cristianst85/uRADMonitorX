using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace uRADMonitorX.Updater.GitHubApi {

    public class Release {

        [JsonPropertyAttribute(PropertyName = "html_url")]
        public String HtmlUrl { get; set; }
        [JsonPropertyAttribute(PropertyName = "tag_name")]
        public String TagName { get; set; }
        [JsonPropertyAttribute(PropertyName = "assets")]
        public ICollection<ReleaseAsset> Assets { get; set; }
    }
}
