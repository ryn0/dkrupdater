using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.Interfaces
{
    public interface IRetrievablePodcast
    {
        int FeedId { get; }

        int MaxFeedsToDownload { get; }

        Uri PodcastUrl { get; }

        string DestinationDirectory { get; }

        List<string> TargetPlaylistPaths { get; }

        List<string> FilterTitlesOn { get; }
    }
}