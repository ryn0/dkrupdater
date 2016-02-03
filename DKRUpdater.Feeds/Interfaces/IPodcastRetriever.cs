using DKRUpdater.Core.Enums;
using DKRUpdater.Feeds.DKRModels;
using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.Interfaces
{
    public interface IPodcastRetriever
    {
        Uri PodcastUri { get; }

        int MaxToDownload { get; }

        PodcastFeedOrigin PodcastFeedOrigin { get; }

        string DestinationDirectoryOfAllPodcastFiles { get; }

        List<string> PlaylistPathsToIncludeIn { get; }

        List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing();
    }
}