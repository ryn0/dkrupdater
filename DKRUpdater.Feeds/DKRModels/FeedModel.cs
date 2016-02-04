using DKRUpdater.Feeds.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.DKRModels
{
    public class Feed : IRetrievablePodcast
    {
        [JsonProperty(PropertyName = "feedId")]
        public int FeedId { get; private set; }

        [JsonProperty(PropertyName = "maxFeedsToDownload")]
        public int MaxFeedsToDownload { get; private set; }

        [JsonProperty(PropertyName = "podcastUrl")]
        public Uri PodcastUrl { get; private set; }

        [JsonProperty(PropertyName = "targetPlaylistPaths")]
        public List<string> TargetPlaylistPaths { get; private set; }

        [JsonProperty(PropertyName = "filterTitlesOn")]
        public List<string> FilterTitlesOn { get; private set; }

        [JsonProperty(PropertyName = "destinationDirectory")]
        public string DestinationDirectory { get; private set; }
    }

    [JsonObject]
    public class RssFeed
    {
        [JsonProperty(PropertyName = "feeds")]
        public List<Feed> Feeds { get; set; }
    }
}
