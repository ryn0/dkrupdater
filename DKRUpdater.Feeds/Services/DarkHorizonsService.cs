using DKRUpdater.Feeds.Interfaces;
using System;
using System.Collections.Generic;
using DKRUpdater.Core.Enums;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.Utilities;
using DKRUpdater.Feeds.Podcasts.BaseRss;

namespace DKRUpdater.Feeds.Services
{
    public class DarkHorizonsService : IPodcastRetriever
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\Dark_Horizons";
            }
        }

        public int MaxToDownload
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

                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.DarkHorizons;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://www.darkhorizonsradio.com/podcasts/shows/Dark_Horizons.xml");
            }
        }

        public List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing()
        {
            var podcastFilesToProcess = PodcastFileProcessor.BuildPodcastFilesForPlaylists<RssRootBase>(
                                                                    PodcastUri,
                                                                    PodcastFeedOrigin,
                                                                    DestinationDirectoryOfAllPodcastFiles,
                                                                    PlaylistPathsToIncludeIn,
                                                                    MaxToDownload);

            return podcastFilesToProcess;

        }
    }
}
