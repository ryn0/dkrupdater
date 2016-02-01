using DKRUpdater.Core.FileSystem;
using DKRUpdater.Core.Logging;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Services;
using DKRUpdater.Playlists;
using System.Collections.Generic;
using System.Linq;

namespace DKRUpdater.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeLogger();

            var downloadedPodcastFilesToProcess = GetDownloadedFilesFromPodcasts();

            PutPodcastFilesInDesinationDirectories(downloadedPodcastFilesToProcess);

            UpdatePlaylistsWithPlacedFiles(downloadedPodcastFilesToProcess);

            Logger.Log("Completed processing all podcasts");
        }

        private static List<DKRPodcastFileToProcess> GetDownloadedFilesFromPodcasts()
        {
            // delete everything first, there might have been a problem
            FileOperations.DeleteAllFilesInDirectory(StringConstants.Mp3DownloadDirectory);

            var downloadedPodcastsToProcess = new List<DKRPodcastFileToProcess>();

            //var danceMachine5000Service = new DanceMachine5000Service();
            //downloadedPodcastsToProcess.AddRange(danceMachine5000Service.GetPodcastFilesForProcessing());

            //var darkHorizonsService = new DarkHorizonsService();
            //downloadedPodcastsToProcess.AddRange(darkHorizonsService.GetPodcastFilesForProcessing());

            //var oontzCastService = new OontzCastService();
            //downloadedPodcastsToProcess.AddRange(oontzCastService.GetPodcastFilesForProcessing());

            var theRequiemService = new TheRequiemService();
            downloadedPodcastsToProcess.AddRange(theRequiemService.GetPodcastFilesForProcessing());

            //var radioBlastFurnanceService = new RadioBlastFurnanceService();
            //downloadedPodcastsToProcess.AddRange(radioBlastFurnanceService.GetPodcastFilesForProcessing());

            //var djRazorGrrlService = new DjRazorGrrlService();
            //downloadedPodcastsToProcess.AddRange(djRazorGrrlService.GetPodcastFilesForProcessing());

            return downloadedPodcastsToProcess;
        }

        private static void PutPodcastFilesInDesinationDirectories(List<DKRPodcastFileToProcess> downloadedPodcastFilesToProcess)
        {
            foreach (var podcastFile in downloadedPodcastFilesToProcess)
            {
                var fromPath = podcastFile.PathToDownloadedMp3;
                var toPath = podcastFile.DestinationPathForMp3;

                FileOperations.MoveFile(fromPath, toPath);
            }
        }

        private static void UpdatePlaylistsWithPlacedFiles(List<DKRPodcastFileToProcess> downloadedPodcastFilesToProcess)
        {
            var distinctPlaylists = downloadedPodcastFilesToProcess.SelectMany(item => item.PlaylistPathsToIncludeIn)
                                                                   .Distinct()
                                                                   .ToList();
           
            // wsh: foreach playlist, get the mp3s that belong on it, read the current playlist, add them if they aren't there
            foreach(var playlist in distinctPlaylists)
            {
                var mp3sForPlaylist = downloadedPodcastFilesToProcess.Where(file => file.PlaylistPathsToIncludeIn.Contains(playlist))
                                                                     .ToList();

                AddMp3sToPlaylist(playlist, mp3sForPlaylist);
            }

        }

        private static void AddMp3sToPlaylist(string playlist, List<DKRPodcastFileToProcess> mp3sForPlaylist)
        {
            PlaylistHelper.WriteNewPlaylist(playlist, mp3sForPlaylist);
        }

        private static void InitializeLogger()
        {
            log4net.Config.BasicConfigurator.Configure();

            Logger.Log("DKRUpdater starting.");
        }
    }
}