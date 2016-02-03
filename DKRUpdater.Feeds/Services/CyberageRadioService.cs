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
    public class CyberageRadioService : IPodcastRetriever
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\cyberageradio";
            }
        }

        public List<string> PlaylistPathsToIncludeIn
        {
            get
            {
                var playlists = new List<string>();

                playlists.Add(@"C:\MP3s\playlist\cyberage.pls");
                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.CyberageRadio;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://www.cyberage.cx/cybercast.php");
            }
        }

        public List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing()
        {
            var podcastFilesToProcess = PodcastFileProcessor.BuildPodcastFilesForPlaylists<RssRootBase>(
                                                                    PodcastUri,
                                                                    PodcastFeedOrigin,
                                                                    DestinationDirectoryOfAllPodcastFiles,
                                                                    PlaylistPathsToIncludeIn);

            return podcastFilesToProcess;
        }
    }
}
