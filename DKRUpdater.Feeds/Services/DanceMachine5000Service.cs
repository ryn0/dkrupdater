using System.Collections.Generic;
using DKRUpdater.Feeds.DKRModels;
using System;
using DKRUpdater.Feeds.Interfaces;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Core.Enums;
using DKRUpdater.Feeds.Utilities;
using DKRUpdater.Feeds.Podcasts.BaseRss;

namespace DKRUpdater.Feeds.Services
{
    public class DanceMachine5000Service : IPodcastRetriever
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\dancemachine5000";
            }
        }

        public List<string> PlaylistPathsToIncludeIn
        {
            get
            {
                var playlists = new List<string>();

                playlists.Add(@"C:\MP3s\playlist\dancemachine5000.pls");
                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.DanceMachine5000;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://dancemachine5000.com/feed/");
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