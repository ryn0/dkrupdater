using DKRUpdater.Core.Enums;
using DKRUpdater.Core.Logging;
using DKRUpdater.Core.Web;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Podcasts.Interfaces;
using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.Utilities
{
    public class PodcastFileProcessor
    {
        public static List<DKRPodcastFileToProcess> BuildPodcastFilesForPlaylists<T>(
            Uri PodcastUri,
            PodcastFeedOrigin podcastFeedOrigin,
            string destinationDirectoryOfAllPodcastFiles,            
            List<string> playlistPathsToIncludeIn,
            int maxToDownload = IntConstants.MaxToDownload,
            List<string> filterOnTitles = null) where T : IRss
        {
            Logger.Log(string.Format("Starting processing of podcasts for: '{0}'", podcastFeedOrigin));

            var rssFeed = DownloadClient.DownloadUrlContentIntoModel<T>(PodcastUri);

            var podcastFilesToProcess = new List<DKRPodcastFileToProcess>();

            // get the amount to do first
            var amountToDownload = GetTotalCountToDownloadForPodcast(
                    podcastFeedOrigin, destinationDirectoryOfAllPodcastFiles, filterOnTitles, rssFeed);

            Logger.Log(string.Format("There are: '{0}' files to download for: '{1}'", amountToDownload, podcastFeedOrigin));

            Logger.Log(string.Format("A maximum of: '{0}' files will be downloaded from: '{1}'", maxToDownload, PodcastUri));

            int quantityDownloaded = 0;

            // then do each
            foreach (var podcastFile in rssFeed.Channel.Item)
            {
                if (quantityDownloaded >= maxToDownload)
                    break;

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

                quantityDownloaded++;
            }

            Logger.Log(string.Format("Completed processing of podcasts for: '{0}'", podcastFeedOrigin));

            return podcastFilesToProcess;
        }

        private static int GetTotalCountToDownloadForPodcast<T>(
            PodcastFeedOrigin podcastFeedOrigin, 
            string destinationDirectoryOfAllPodcastFiles, 
            List<string> filterOnTitles, 
            T rssFeed) where T : IRss
        {
            var countOfFeedsToDownload = 0;

            foreach (var podcastFile in rssFeed.Channel.Item)
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
                countOfFeedsToDownload++;
            }

            return countOfFeedsToDownload;
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
