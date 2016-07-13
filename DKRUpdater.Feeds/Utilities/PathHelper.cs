using DKRUpdater.Core;
using DKRUpdater.Core.Configs;
using DKRUpdater.Core.Constants;
using DKRUpdater.Core.Conventions;
using DKRUpdater.Core.FileConversion;
using DKRUpdater.Core.Logging;
using System;
using System.IO;

namespace DKRUpdater.Feeds.Utilities
{
    public class PathHelper
    {
        public static string SetDestinationPathForMp3(string pathToDownloadedMp3, string destinationDirectoryOfAllPodcastFiles)
        {
            return string.Format(@"{0}\{1}",
                destinationDirectoryOfAllPodcastFiles,
                Path.GetFileName(pathToDownloadedMp3));
        }

        public static string SetPathToMp3(string downloadedFilePath)
        {
            var soundHelper = new SoundFileHelper();

            return soundHelper.ToMp3(downloadedFilePath);
        }

        public static string DownloadFilePath(
            Uri podcastFileUrl,
            DateTime releaseDateOfPodcast,
            int feedId)
        {
            var path = string.Format(@"{0}\{1}",
                PlaylistConfigs.Mp3DownloadDirectory,
                FilenameConventions.DownloadFilenameFormatter(podcastFileUrl, feedId, releaseDateOfPodcast));

            return path;
        }

        public static bool PodcastExists
            (Uri podcastFileUrl,
            DateTime releaseDateOfPodcast,
            int feedId,
            string destinationDirectoryOfAllPodcastFiles)
        {
            var fileToDownload = FilenameConventions.DownloadFilenameFormatter(podcastFileUrl, feedId, releaseDateOfPodcast);
          
            var path = string.Format(@"{0}\{1}",
                            destinationDirectoryOfAllPodcastFiles,
                            fileToDownload);

            Log.Debug("Checking if destination file at: '{0}' exists...", path);

            return File.Exists(path);
        }
    }
}
