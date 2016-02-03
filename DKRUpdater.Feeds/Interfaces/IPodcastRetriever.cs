using DKRUpdater.Core.Enums;
using DKRUpdater.Feeds.DKRModels;
using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.Interfaces
{
    public interface IRetrievablePodcast
    {
        Uri PodcastUri { get; }

        int MaxNewToDownload { get; }

        PodcastFeedOrigin PodcastFeedOrigin { get; }

        string DestinationDirectoryOfAllPodcastFiles { get; }

        List<string> PlaylistPathsToIncludeIn { get; }

        List<string> FilterTitlesOn { get; }
        

        List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing();
    }
}