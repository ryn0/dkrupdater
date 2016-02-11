using DKRUpdater.Feeds.Interfaces;
using System.Collections.Generic;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Utilities;
using DKRUpdater.Feeds.Podcasts.BaseRss;

namespace DKRUpdater.Feeds.Services
{
    public class FeedRetrievalService : IFeedRetrievalService
    {
        public List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing(List<IRetrievablePodcast> podcasts)
        {
            var podcastFilesToProcess = new List<DKRPodcastFileToProcess>();

            var podcastFileProcessor = new PodcastFileProcessor();

            foreach (var podcast in podcasts)
            {
                var processablePodcasts =
                    podcastFileProcessor.BuildPodcastFilesForPlaylists<RssRootBase>(podcast);

                if (HasNoPodcasts(processablePodcasts))
                    continue;

                podcastFilesToProcess.AddRange(processablePodcasts);
            }

            return podcastFilesToProcess;
        }

        private bool HasNoPodcasts(List<DKRPodcastFileToProcess> processablePodcasts)
        {
            return processablePodcasts == null || processablePodcasts.Count == 0;
        }
    }
}
