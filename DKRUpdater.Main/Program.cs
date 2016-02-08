using DKRUpdater.Core.Constants;
using DKRUpdater.Core.FileSystem;
using DKRUpdater.Core.Logging;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Interfaces;
using DKRUpdater.Feeds.Services;
using DKRUpdater.Playlists;
using DKRUpdater.Playlists.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DKRUpdater.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeLogger();

            CreateAllRequiredDirectories();

            var podcastFeeds = GetPodcastFeeds();
            
            var downloadedPodcastFilesToProcess = GetDownloadedFilesFromPodcasts(podcastFeeds);

            PutPodcastFilesInDesinationDirectories(downloadedPodcastFilesToProcess);

            var playlistConfiguration = PlaylistConfigs.BuildFromConfigs();

            UpdatePlaylistsWithPlacedFiles(downloadedPodcastFilesToProcess, playlistConfiguration);

            Log.Debug("Completed processing all podcasts!");
        }

        private static void CreateAllRequiredDirectories()
        {
            Directories.CreateDirectoryIfNotExists(StringConstants.Mp3DownloadDirectory);
            Directories.CreateDirectoryIfNotExists(StringConstants.Mp3LogsDirectory);
            Directories.CreateDirectoryIfNotExists(StringConstants.Mp3MusicDirectory);
            Directories.CreateDirectoryIfNotExists(StringConstants.Mp3PlaylistsDirectory);
        }

        private static List<IRetrievablePodcast> GetPodcastFeeds()
        {
            var pathToFeedFile = string.Format(@"{0}\feeds.json", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            var feedReaderService = new FeedReaderService();
            var feeds = feedReaderService.GetFeeds(pathToFeedFile);

            return feeds;
        }

        private static List<DKRPodcastFileToProcess> GetDownloadedFilesFromPodcasts(List<IRetrievablePodcast> podcasts)
        {
            // clean the diretory of any partially downloaded files
            FileOperations.DeleteAllFilesInDirectory(StringConstants.Mp3DownloadDirectory);

            var feedRetrievalService = new FeedRetrievalService();

            var downloadedPodcastsToProcess = feedRetrievalService.GetPodcastFilesForProcessing(podcasts);
            
            return downloadedPodcastsToProcess;
        }

        private static void PutPodcastFilesInDesinationDirectories(List<DKRPodcastFileToProcess> downloadedPodcasts)
        {
            foreach (var podcastFile in downloadedPodcasts)
            {
                var fromPath = podcastFile.PathToDownloadedMp3;
                var toPath = podcastFile.DestinationPathForMp3;

                FileOperations.MoveFile(fromPath, toPath);
            }
        }

        private static void UpdatePlaylistsWithPlacedFiles(
            List<DKRPodcastFileToProcess> downloadedPodcasts, 
            PlaylistConfigs playlistConfigs)
        {
            var distinctPlaylists = downloadedPodcasts.SelectMany(item => item.PlaylistPathsToIncludeIn)
                                                      .Distinct()
                                                      .ToList();
           
            // wsh: foreach playlist, get the mp3s that belong on it, read the current playlist, add them if they aren't there
            foreach(var playlist in distinctPlaylists)
            {
                var mp3sForPlaylist = downloadedPodcasts.Where(file => file.PlaylistPathsToIncludeIn.Contains(playlist))
                                                                                                    .ToList();

                AddMp3sToPlaylist(playlist, mp3sForPlaylist, playlistConfigs.MaxInNewMusicPlaylist);
            }
        }
        
        private static void AddMp3sToPlaylist(
            string playlist, 
            List<DKRPodcastFileToProcess> mp3sForPlaylist, 
            int maxInNewMusicPlaylist)
        {
            PlaylistHelper.WriteNewPlaylist(playlist, mp3sForPlaylist, maxInNewMusicPlaylist);
        }

        private static void InitializeLogger()
        {
            log4net.Config.BasicConfigurator.Configure();

            Log.Debug("++++++++++++++++++++++++++++++++++++++");
            Log.Debug("DKRUpdater (version 0.2.1) starting...");
            Log.Debug("Current time UTC: '{0}'", DateTime.UtcNow.ToString("u"));
            Log.Debug("Current directory is: '{0}'", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            Log.Debug("--------------------------------------------------------------");
            Log.Debug("Das Klub Radio Updater - Podcast Retrievel and Placement Task");
            Log.Debug("Das Klub | 2016");
            Log.Debug("--------------------------------------------------------------");
        }
    }
}