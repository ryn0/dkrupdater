using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKRUpdater.Feeds.DKRModels
{

    public class Feed
    {
        [JsonProperty(PropertyName = "feedId")]
        public int FeedId { get; set; }

        [JsonProperty(PropertyName = "maxFeedsToDownload")]
        public int MaxFeedsToDownload { get; set; }

        [JsonProperty(PropertyName = "podcastUrl")]
        public string PodcastUrl { get; set; }

        [JsonProperty(PropertyName = "targetPlaylistPaths")]
        public List<string> TargetPlaylistPaths { get; set; }

        [JsonProperty(PropertyName = "filterTitlesOn")]
        public List<string> FilterTitlesOn { get; set; }

    }

    [JsonObject]
    public class FeedModel
    {
        [JsonProperty(PropertyName = "feeds")]
        public List<Feed> Feeds { get; set; }
    }
}
