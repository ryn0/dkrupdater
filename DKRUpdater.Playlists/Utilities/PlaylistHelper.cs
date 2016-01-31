using DKRUpdater.Core.Logging;
using DKRUpdater.Core.StringParsing;
using DKRUpdater.Feeds.Constants;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Playlists.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DKRUpdater.Playlists
{
    public class PlaylistHelper
    {
        const int QuantityOfStationFilesInNewMusicPlaylist = 4;
        const int MaxFilesInNewMusicPlaylist = 12;

        const string PlaylistFormat =
@"[playlist]
{0}
NumberOfEntries={1}
Version=2";    

        public static int SecondsInMp3(string path)
        {
            var mp3Track = TagLib.File.Create(path);

            var seconds = Convert.ToInt32(Math.Floor(mp3Track.Properties.Duration.TotalSeconds));

            return seconds;
        }

        public static List<PlaylistFile> GetPlaylist(string playlistPath)
        {
            if (!File.Exists(playlistPath))
            {
                Logger.LogError(string.Format("Cannot find playlist at: '{0}'", playlistPath), new Exception());

                CreateDefaultPlaylist(playlistPath);
            }

            var playlist = new List<PlaylistFile>();

            var content = new string[]{};

            try
            {
                content = File.ReadAllLines(playlistPath);
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Cannot read playlist at: '{0}'", playlistPath), ex);
            }

            for (int i = 1; i < content.Length - 3; i = i + 3)
            {
                var mp3InPlaylist = GetPlaylistFileFromPlaylistLine(content[i], content[i + 1], content[i + 2]);

                playlist.Add(mp3InPlaylist);
            }

            return playlist;
        }

        private static void CreateDefaultPlaylist(string playlistPath)
        {
            var defaultContent = 
@"[playlist]
File1=C:\MP3s\music\station\dk radio 2.mp3
Title1=dk radio 2
Length1=5
NumberOfEntries=1
Version=2";
            try
            {
                Logger.Log(string.Format("Creating new playlist at: '{0}'", playlistPath));

                using (var sw = File.CreateText(playlistPath))
                {
                    sw.WriteLine(defaultContent);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Cannot create new playlist file: '{0}'", playlistPath), ex);
            }

            Logger.Log(string.Format("Completed creating playlist at: '{0}'", playlistPath));
        }

        private static PlaylistFile GetPlaylistFileFromPlaylistLine(string file, string title, string length)
        {
            var fileToUse = file.Split('=')[1];
            var titleToUse = title.Split('=')[1];
            var lengthToUse = length.Split('=')[1];

            var playlistFile = new PlaylistFile()
            {
                File = fileToUse,
                Length = Convert.ToInt32(lengthToUse),
                Title = titleToUse
            };

            return playlistFile;
        }

        public static void WriteNewPlaylist(string pathToPlaylist, List<DKRPodcastFileToProcess> mp3sForPlaylist)
        {
            Logger.Log(string.Format("Writing new playlist to: '{0}'", pathToPlaylist));

            var existingPlaylist = GetPlaylist(pathToPlaylist);

            var newPlaylist = IsNewMusicPlaylist(pathToPlaylist) ?
                                   CreateNewMusicPlaylist(mp3sForPlaylist, existingPlaylist) :
                                   CreateStandardPlaylist(mp3sForPlaylist, existingPlaylist);

            var playlistTextFile = GetPlaylistFilesStringForPlaylist(newPlaylist);

            try
            {
                File.WriteAllText(pathToPlaylist, playlistTextFile);
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed to write playlist to: '{0}'", pathToPlaylist), ex);
            }

            Logger.Log(string.Format("Completed writing new playlist to: '{0}'", pathToPlaylist));
        }

        private static List<PlaylistFile> CreateNewMusicPlaylist(List<DKRPodcastFileToProcess> mp3sForPlaylist, List<PlaylistFile> existingPlaylist)
        {
            var datedExistingPlaylist = GetExistingPlaylistWithDates(existingPlaylist);
            var datedNewPlaylist = GetNewPlaylistWithDates(mp3sForPlaylist);

            var possiblePlaylistFiles = new List<DatedPlaylistFile>();

            possiblePlaylistFiles.AddRange(datedExistingPlaylist);
            possiblePlaylistFiles.AddRange(datedNewPlaylist);

            var mostRecentFiles = possiblePlaylistFiles.OrderByDescending(file => file.PublishDate)
                                                       .Take(MaxFilesInNewMusicPlaylist + QuantityOfStationFilesInNewMusicPlaylist);

            var newPlaylist = new List<PlaylistFile>();

            foreach(var newFileToInclude in mostRecentFiles)
            {
                newPlaylist.Add(newFileToInclude);
            }

            return newPlaylist;
        }

        private static List<DatedPlaylistFile> GetNewPlaylistWithDates(List<DKRPodcastFileToProcess> mp3sForPlaylist)
        {
            var datedPlaylist = new List<DatedPlaylistFile>();

            foreach (var entry in mp3sForPlaylist)
            {
                var datedPlaylistFile = new DatedPlaylistFile()
                {
                    File = entry.DestinationPathForMp3,
                    Length = SecondsInMp3(entry.DestinationPathForMp3),
                    Title = Path.GetFileName(entry.DestinationPathForMp3)
                };

                datedPlaylistFile.PublishDate = entry.ReleaseDateOfPodcastFile;

                datedPlaylist.Add(datedPlaylistFile);
            }

            return datedPlaylist;
        }

        private static List<DatedPlaylistFile> GetExistingPlaylistWithDates(List<PlaylistFile> existingPlaylist)
        {
            var datedPlaylist = new List<DatedPlaylistFile>();

            foreach(var entry in existingPlaylist)
            {
                var datedPlaylistFile = new DatedPlaylistFile()
                {
                    File = entry.File,
                    Length = entry.Length,
                    Title = entry.Title
                };

                datedPlaylistFile.PublishDate = FileNameParsing.GetDateFromFilePath(entry.File);

                datedPlaylist.Add(datedPlaylistFile);
            }

            return datedPlaylist;
        }

        private static List<PlaylistFile> CreateStandardPlaylist(List<DKRPodcastFileToProcess> mp3sForPlaylist, List<PlaylistFile> existingPlaylist)
        {
            var newPlaylist = new List<PlaylistFile>();

            foreach (var newFile in mp3sForPlaylist)
            {
                var pathWithoutDriveLetter = GetMp3PathWithoutDriveLetter(newFile.DestinationPathForMp3);

                var existingFile = existingPlaylist.FirstOrDefault(item => item.File.Contains(pathWithoutDriveLetter));

                if (existingFile != null)
                    continue;

                var playlistItem = GetPlaylistFileFromProdcastFile(newFile);

                newPlaylist.Add(playlistItem);
            }

            newPlaylist.AddRange(existingPlaylist);

            return newPlaylist;
        }

        private static string GetMp3PathWithoutDriveLetter(string destinationPathForMp3)
        {
            var fileInfo = new FileInfo(destinationPathForMp3);
            var drive = Path.GetPathRoot(fileInfo.FullName);

            return destinationPathForMp3.Replace(drive, string.Empty);
        }

        private static string GetPlaylistFilesStringForPlaylist(List<PlaylistFile> newPlaylist)
        {
            var sb = new StringBuilder();

            var index = 1;

            foreach(var playlistEntry in newPlaylist)
            {
                sb.AppendFormat(@"File{0}={1}", index, playlistEntry.File);
                sb.AppendLine();
                sb.AppendFormat(@"Title{0}={1}", index, playlistEntry.Title);
                sb.AppendLine();
                sb.AppendFormat(@"Length{0}={1}", index, playlistEntry.Length);
                sb.AppendLine();

                index++;
            }

            var entries = sb.ToString();

            entries = entries.TrimEnd('\r', '\n');

            var formattedPlaylist = string.Format(PlaylistFormat, entries, newPlaylist.Count);

            return formattedPlaylist;
        }

        private static PlaylistFile GetPlaylistFileFromProdcastFile(DKRPodcastFileToProcess newFile)
        {
            var playlistFile = new PlaylistFile();

            playlistFile.File = newFile.DestinationPathForMp3;
            playlistFile.Title = Path.GetFileName(newFile.DestinationPathForMp3);
            playlistFile.Length = SecondsInMp3(newFile.DestinationPathForMp3);

            return playlistFile;
        }

        private static bool IsNewMusicPlaylist(string pathToPlaylist)
        {
            return pathToPlaylist.ToLower().Contains(StringConstants.NewMusicPlaylist.ToLower());
        }
    }
}
