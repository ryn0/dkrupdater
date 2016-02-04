using DKRUpdater.Feeds.DKRModels;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.Interfaces
{
    public interface IFeedRetrievalService
    {
        List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing(List<IRetrievablePodcast> podcasts);
    }
}