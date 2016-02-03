using DKRUpdater.Core.Enums;
using DKRUpdater.Core.Logging;
using DKRUpdater.Core.Web;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Podcasts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DKRUpdater.Feeds.Utilities
{
    public class PodcastFileProcessor
    {
        public static List<DKRPodcastFileToProcess> BuildPodcastFilesForPlaylists<T>(
            Uri PodcastUri,
            PodcastFeedOrigin podcastFeedOrigin,
            string destinationDirectoryOfAllPodcastFiles,            
            List<string> playlistPathsToIncludeIn,
            int maxNewToDownload = IntConstants.MaxNewToDownload,
            List<string> filterOnTitles = null) where T : IRss
        {
            Logger.Log("Starting processing of podcasts for: '{0}'", podcastFeedOrigin);

            var rssFeed = DownloadClient.DownloadUrlContentIntoModel<T>(PodcastUri);

            var podcastFilesToProcess = new List<DKRPodcastFileToProcess>();
            
            Logger.Log("A maximum of: '{0}' files will be downloaded from: '{1}'", maxNewToDownload, PodcastUri);

            var podcastsToProcess = rssFeed.Channel.Item.OrderByDescending(x => Convert.ToDateTime(x.PubDate))
                                                        .Take(maxNewToDownload);

            foreach (var podcastFile in podcastsToProcess)
            {                
                if (!IsAllowedPodcast(podcastFile.Title, filterOnTitles))
                    continue;

                var podcastFileUrl = new Uri(podcastFile.Enclosure.Url);
                var releaseDateOfPodcast = Convert.ToDateTime(podcastFile.PubDate);

                if (PathHelper.PodcastExists(
                                        podcastFileUrl,
                                        releaseDateOfPodcast,
                                        podcastFeedOrigin,
                                        destinationDirectoryOfAllPodcastFiles))
                {
                    continue;
                }

                var podcastFileToProcess = new DKRPodcastFileToProcess()
                {
                    ReleaseDateOfPodcastFile = releaseDateOfPodcast,
                    PlaylistPathsToIncludeIn = playlistPathsToIncludeIn
                };

                var downloadedFilePath = PathHelper.DownloadFilePath(podcastFileUrl, releaseDateOfPodcast, podcastFeedOrigin);

                if (!DownloadClient.DownloadFile(podcastFileUrl, downloadedFilePath))
                {
                    continue;
                }

                podcastFileToProcess.PathToDownloadedMp3 = PathHelper.SetPathToMp3(downloadedFilePath);
                podcastFileToProcess.DestinationPathForMp3 = PathHelper.SetDestinationPathForMp3(
                                                                            podcastFileToProcess.PathToDownloadedMp3,
                                                                            destinationDirectoryOfAllPodcastFiles);

                podcastFilesToProcess.Add(podcastFileToProcess);
            }

            Logger.Log("Completed processing of podcasts for: '{0}'", podcastFeedOrigin);

            return podcastFilesToProcess;
        }

        private static bool IsAllowedPodcast(string title, List<string> filterOnTitles)
        {
            if (filterOnTitles == null || filterOnTitles.Count == 0)
                return true;

            foreach(var allowedTitle in filterOnTitles)
            {
                if (title.ToLower().Contains(allowedTitle.ToLower()))
                    return true;
            }

            return false;
        }
    }
}
