using DKRUpdater.Feeds.Interfaces;
using System;
using System.Collections.Generic;
using DKRUpdater.Core.Enums;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Utilities;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.Podcasts.BaseRss;

namespace DKRUpdater.Feeds.Services
{
    public class OontzCastService : IPodcastRetriever
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\oontzcast";
            }
        }

        public List<string> PlaylistPathsToIncludeIn
        {
            get
            {
                var playlists = new List<string>();

                playlists.Add(@"C:\MP3s\playlist\oontzcast.pls");
                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.OontzCast;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://oontzcast.podbean.com/feed/");
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
