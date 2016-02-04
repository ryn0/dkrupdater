using DKRUpdater.Feeds.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.DKRModels
{
    public class Feed : IRetrievablePodcast
    {
        [JsonProperty(PropertyName = "feedId")]
        public int FeedId { get;  set; }

        [JsonProperty(PropertyName = "maxFeedsToDownload")]
        public int MaxFeedsToDownload { get;  set; }

        [JsonProperty(PropertyName = "podcastUrl")]
        public Uri PodcastUrl { get;  set; }

        [JsonProperty(PropertyName = "targetPlaylistPaths")]
        public List<string> TargetPlaylistPaths { get;  set; }

        [JsonProperty(PropertyName = "filterTitlesOn")]
        public List<string> FilterTitlesOn { get;  set; }

        [JsonProperty(PropertyName = "destinationDirectory")]
        public string DestinationDirectory { get;  set; }
    }

    [JsonObject]
    public class RssFeed
    {
        [JsonProperty(PropertyName = "feeds")]
        public List<Feed> Feeds { get; set; }
    }
}
