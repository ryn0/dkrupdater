using DKRUpdater.Core;
using DKRUpdater.Core.Conventions;
using DKRUpdater.Core.Enums;
using DKRUpdater.Core.FileConversion;
using DKRUpdater.Feeds.Constants;
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
            if (FileHelper.IsFileM4a(downloadedFilePath))
            {
                var pathToConvertedMp3 = M4A2MP3.ConvertMp4ToMp3(downloadedFilePath);

                return pathToConvertedMp3;
            }
            else
            {
                return downloadedFilePath;
            }
        }

        public static string DownloadFilePath(
            Uri podcastFileUrl,
            DateTime releaseDateOfPodcast,
            PodcastFeedOrigin podcastFeedOrigin)
        {
            var path = string.Format(@"{0}\{1}",
                StringConstants.Mp3DownloadDirectory,
                FilenameConventions.DownloadFilenameFormatter(podcastFileUrl, podcastFeedOrigin, releaseDateOfPodcast));

            return path;
        }

        public static bool PodcastExists
            (Uri podcastFileUrl,
            DateTime releaseDateOfPodcast,
            PodcastFeedOrigin podcastFeedOrigin,
            string destinationDirectoryOfAllPodcastFiles)
        {
            var fileToDownload = FilenameConventions.DownloadFilenameFormatter(podcastFileUrl, podcastFeedOrigin, releaseDateOfPodcast);

            if (FileHelper.IsFileM4a(fileToDownload))
            {
                fileToDownload = fileToDownload.Replace(".m4a", ".mp3");
            }

            var path = string.Format(@"{0}\{1}",
                            destinationDirectoryOfAllPodcastFiles,
                            fileToDownload);

            return File.Exists(path);
        }
    }
}
