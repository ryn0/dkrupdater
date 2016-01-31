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
    public class DjRazorGrrlService : IPodcastRetriever
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\distorted_circuitry";
            }
        }

        public List<string> PlaylistPathsToIncludeIn
        {
            get
            {
                var playlists = new List<string>();

                playlists.Add(@"C:\MP3s\playlist\distorted_circuitry.pls");
                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.DjRazorGrrl;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://feeds.feedburner.com/djrazorgrrl?format=xml");
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
