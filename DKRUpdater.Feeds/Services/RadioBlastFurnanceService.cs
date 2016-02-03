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
    public class RadioBlastFurnanceService : IRetrievablePodcast
    {
        public string DestinationDirectoryOfAllPodcastFiles
        {
            get
            {
                return @"C:\MP3s\music\radioblastfurnace";
            }
        }

        public List<string> FilterTitlesOn
        {
            get
            {
                throw new NotImplementedException();
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

                playlists.Add(@"C:\MP3s\playlist\radioblastfurnance.pls");
                playlists.Add(StringConstants.NewMusicPlaylist);

                return playlists;
            }
        }

        public PodcastFeedOrigin PodcastFeedOrigin
        {
            get
            {
                return PodcastFeedOrigin.RadioBlastFurnance;
            }
        }

        public Uri PodcastUri
        {
            get
            {
                return new Uri("http://feeds.feedburner.com/RadioBlastFurnace");
            }
        }

        public List<DKRPodcastFileToProcess> GetPodcastFilesForProcessing()
        {
            var podcastFilesToProcess = PodcastFileProcessor.BuildPodcastFilesForPlaylists<RssRootBase>(
                                                                    PodcastUri,
                                                                    PodcastFeedOrigin,
                                                                    DestinationDirectoryOfAllPodcastFiles,
                                                                    PlaylistPathsToIncludeIn,
                                                                    MaxNewToDownload);

            return podcastFilesToProcess;
        }

    }
}