using DKRUpdater.Core.Enums;
using DKRUpdater.Core.Logging;
using DKRUpdater.Core.Web;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Podcasts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DKRUpdater.Feeds.Podcasts.BaseRss;

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
            DKRlogger.Debug("Starting processing of podcasts for: '{0}'", podcastFeedOrigin);

            var rssFeed = DownloadClient.DownloadUrlContentIntoModel<T>(PodcastUri);

            var podcastFilesToProcess = new List<DKRPodcastFileToProcess>();
            
            DKRlogger.Debug("A maximum of: '{0}' files will be downloaded from: '{1}'", maxNewToDownload, PodcastUri);

            var podcastItems = rssFeed.Channel.Item;

            DKRlogger.Debug("A total of: '{0}' podcasts were found on this feed.", podcastItems.Count());

            var filteredPodcats = FilterPodcasts(podcastItems, filterOnTitles);

            var podcastsToProcess = filteredPodcats.OrderByDescending(x => Convert.ToDateTime(x.PubDate))
                                                   .Take(maxNewToDownload);

            foreach (var podcastFile in podcastsToProcess)
            {                
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

            DKRlogger.Debug("Completed processing of podcasts for: '{0}'", podcastFeedOrigin);

            return podcastFilesToProcess;
        }

        private static List<Item> FilterPodcasts(List<Item> podcasts, List<string> filterOnTitles)
        {
            if (IsMissingFilters(filterOnTitles))
            {
                return podcasts;
            }

            var result = from p in podcasts
                     where filterOnTitles.Any(val => p.Title.Contains(val))
                     select p;

            return result.ToList();
        }

        private static bool IsMissingFilters(List<string> filterOnTitles)
        {
            return filterOnTitles == null || filterOnTitles.Count() <= 0;
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
