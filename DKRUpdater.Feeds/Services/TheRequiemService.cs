using System;
using System.Collections.Generic;
using DKRUpdater.Core.Enums;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Interfaces;
using DKRUpdater.Feeds.Utilities;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.Podcasts.BaseRss;

namespace DKRUpdater.Feeds.Services
{
    public class TheRequiemService : IPodcastRetriever
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\therequiem";
            }
        }

        public int MaxNewToDownload
        {
            get
            {
                return 25;
            }
        }

        public List<string> PlaylistPathsToIncludeIn
        {
            get
            {
                var playlists = new List<string>();

                playlists.Add(@"C:\MP3s\playlist\generalpodcasts.pls");
                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.TheRequiem;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://therequiem.libsyn.com/rss");
            }
        }
 
        public List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing()
        {
            var podcastFilesToProcess = PodcastFileProcessor.BuildPodcastFilesForPlaylists<RssRootBase>(
                                                                    PodcastUri,
                                                                    PodcastFeedOrigin,
                                                                    DestinationDirectoryOfAllPodcastFiles,
                                                                    PlaylistPathsToIncludeIn,
                                                                    MaxNewToDownload,
                                                                    Filters());

            return podcastFilesToProcess;
        }

        private List<string> Filters()
        {
            var djDeparved = "DJ Depraved";
            var darkIndustrialFrequencies = "Dark Industrial Frequencies";

            var includeList = new List<string>();
            includeList.Add(djDeparved);
            includeList.Add(darkIndustrialFrequencies);

            return includeList;
        }
    }
}
