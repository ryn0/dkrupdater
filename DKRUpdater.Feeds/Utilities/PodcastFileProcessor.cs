using DKRUpdater.Core.Logging;
using DKRUpdater.Core.Web;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Podcasts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DKRUpdater.Feeds.Podcasts.BaseRss;
using DKRUpdater.Feeds.Interfaces;
using System.Text;
using DKRUpdater.Core.Constants;

namespace DKRUpdater.Feeds.Utilities
{
    public class PodcastFileProcessor
    {
        public List<DKRPodcastFileToProcess> BuildPodcastFilesForPlaylists<T>(IRetrievablePodcast podcast) where T : IRss
        {
            return BuildPodcastFilesForPlaylists<RssRootBase>(
                    podcast.PodcastUrl,
                    podcast.FeedId,
                    podcast.DestinationDirectory,
                    podcast.TargetPlaylistPaths,
                    podcast.MaxFeedsToDownload,
                    podcast.FilterTitlesOn);
        }

        private List<DKRPodcastFileToProcess> BuildPodcastFilesForPlaylists<T>(
            Uri PodcastUri,
            int feedId,
            string destinationDirectoryOfAllPodcastFiles,            
            List<string> playlistPathsToIncludeIn,
            int maxNewToDownload = IntConstants.MaxNewToDownload,
            List<string> filterOnTitles = null) where T : IRss
        {
            Log.Debug("-----------------------------------------------------");
            Log.Debug("Starting processing of podcasts for feed id: '{0}'...", feedId);

            var rssFeed = DownloadClient.DownloadUrlContentIntoModel<T>(PodcastUri);

            var podcastItems = rssFeed.Channel.Item;

            Log.Debug("A total of: '{0}' podcasts were found on this feed.", podcastItems.Count());
            Log.Debug("A maximum of: '{0}' files will be downloaded from: '{1}'.", maxNewToDownload, PodcastUri);
            Log.Debug("Filtering podcasts...");

            var podcastFilesToProcess = new List<DKRPodcastFileToProcess>();

            var filteredPodcats = FilterPodcasts(podcastItems, filterOnTitles);

            var podcastsToProcess = filteredPodcats.OrderByDescending(x => Convert.ToDateTime(x.PubDate))
                                                   .Take(maxNewToDownload);

            Log.Debug("A total of: '{0}' podcasts will be checked.", podcastsToProcess.Count());

            foreach (var podcastFile in podcastsToProcess)
            {                
                var podcastFileUrl = new Uri(podcastFile.Enclosure.Url);
                var releaseDateOfPodcast = Convert.ToDateTime(podcastFile.PubDate);

                if (PathHelper.PodcastExists(
                                        podcastFileUrl,
                                        releaseDateOfPodcast,
                                        feedId,
                                        destinationDirectoryOfAllPodcastFiles))
                {
                    Log.Debug("Podcast at: '{0}' already exists in the destination.", podcastFileUrl);
                    continue;
                }

                var podcastFileToProcess = new DKRPodcastFileToProcess()
                {
                    ReleaseDateOfPodcastFile = releaseDateOfPodcast,
                    PlaylistPathsToIncludeIn = playlistPathsToIncludeIn
                };

                var downloadedFilePath = PathHelper.DownloadFilePath(podcastFileUrl, releaseDateOfPodcast, feedId);

                if (!DownloadClient.DownloadFile(podcastFileUrl, downloadedFilePath))
                {
                    Log.Debug("Skipping file at: '{0}' which could not be downloaded.", podcastFileUrl);
                    continue;
                }

                podcastFileToProcess.PathToDownloadedMp3 = PathHelper.SetPathToMp3(downloadedFilePath);
                podcastFileToProcess.DestinationPathForMp3 = PathHelper.SetDestinationPathForMp3(
                                                                            podcastFileToProcess.PathToDownloadedMp3,
                                                                            destinationDirectoryOfAllPodcastFiles);

                podcastFilesToProcess.Add(podcastFileToProcess);
            }

            Log.Debug("Completed processing of podcasts for feed id: '{0}'.", feedId);

            return podcastFilesToProcess;
        }

        private List<Item> FilterPodcasts(List<Item> podcasts, List<string> filterOnTitles)
        {
            if (IsMissingFilters(filterOnTitles))
            {
                return podcasts;
            }

            var filters = new StringBuilder();

            foreach(var title in filterOnTitles)
            {
                filters.AppendFormat("{0}|", title);
            }

            Log.Debug("Filtering on: '{0}'", filters.ToString());


            var result = from p in podcasts
                         where filterOnTitles.Any(val => p.Title.ToLower()
                                                                .Contains(val.ToLower()))
                         select p;

            return result.ToList();
        }

        private bool IsMissingFilters(List<string> filterOnTitles)
        {
            return filterOnTitles == null || filterOnTitles.Count() <= 0;
        }
    }
}
