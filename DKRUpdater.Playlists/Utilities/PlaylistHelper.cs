using DKRUpdater.Core.Constants;
using DKRUpdater.Core.Logging;
using DKRUpdater.Core.StringParsing;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Playlists.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DKRUpdater.Playlists
{
    public class PlaylistHelper
    {
        const string FileKey = "File";
        const string TitleKey = "Title";
        const string LengthKey = "Length"; 

        const string PlaylistFormat =
@"[playlist]
{0}
NumberOfEntries={1}
Version=2";    

        public static int SecondsInMp3(string path)
        {
            if (!File.Exists(path))
            {
                Log.Error(string.Format("Cannot find file to get seconds: '{0}'", path), new Exception());

                return 0;
            }

            var mp3Track = TagLib.File.Create(path);

            var seconds = Convert.ToInt32(Math.Floor(mp3Track.Properties.Duration.TotalSeconds));

            return seconds;
        }

        public static List<PlaylistFile> GetPlaylist(string playlistPath)
        {
            if (!File.Exists(playlistPath))
            {
                Log.Error(string.Format("Cannot find existing playlist at: '{0}'", playlistPath), new Exception());

                CreateDefaultPlaylist(playlistPath);
            }

            var playlist = new List<PlaylistFile>();
            var content = new string[]{};

            try
            {
                content = File.ReadAllLines(playlistPath);
                content = content.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Cannot read playlist at: '{0}'", playlistPath), ex);
            }

            try
            {
                Log.Debug("Starting iterating lines in existing playlist: '{0}'...", playlistPath);

                for (int i = 1; i < content.Length - 3; i = i + 3)
                {
                    var file = content[i];
                    var title = content[i + 1];
                    var length = content[i + 2];

                    if (!IsCorrectFormat(file, title, length))
                        continue;

                    var mp3InPlaylist = GetPlaylistFileFromPlaylistLine(file, title, length);

                    playlist.Add(mp3InPlaylist);
                }

                Log.Debug(string.Format("Completed iterating lines in existing playlist: '{0}'", playlistPath));
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error reading playlist file: '{0}'", playlistPath), ex);
            }

            return playlist;
        }

        private static bool IsCorrectFormat(string file, string title, string lenth)
        {
            return
                (file.ToLower().Contains(FileKey.ToLower()) &&
                title.ToLower().Contains(TitleKey.ToLower()) &&
                lenth.ToLower().Contains(LengthKey.ToLower()));
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
                Log.Debug(string.Format("Creating new playlist at: '{0}'", playlistPath));

                using (var sw = File.CreateText(playlistPath))
                {
                    sw.WriteLine(defaultContent);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Cannot create new playlist file: '{0}'", playlistPath), ex);
            }

            Log.Debug("Completed creating playlist at: '{0}'", playlistPath);
        }

        public static void WriteNewPlaylist(string playlist, List<DKRPodcastFileToProcess> mp3sForPlaylist)
        {
            WriteNewPlaylist(playlist, mp3sForPlaylist);
        }

        private static PlaylistFile GetPlaylistFileFromPlaylistLine(string file, string title, string length)
        {
            var fileToUse = file.Split('=')[1];
            var titleToUse = title.Split('=')[1];
            var lengthToUse = length.Split('=')[1];

            int lengthInSeconds = 0;

            if (!int.TryParse(lengthToUse, out lengthInSeconds))
            {
                Log.Error(string.Format("Value '{0}' is not an int", lengthToUse), new Exception());
            }

            var playlistFile = new PlaylistFile()
            {
                File = fileToUse,
                Length = lengthInSeconds,
                Title = titleToUse
            };

            return playlistFile;
        }

        public static void WriteNewPlaylist(string pathToPlaylist, List<DKRPodcastFileToProcess> mp3sForPlaylist, int maxFilesInNewMusicPlaylist)
        {
            var existingPlaylist = GetPlaylist(pathToPlaylist);

            Log.Debug(string.Format("Started writing new playlist to: '{0}'", pathToPlaylist));
            
            var newPlaylist = IsNewMusicPlaylist(pathToPlaylist) ?
                                   CreateNewMusicPlaylist(mp3sForPlaylist, existingPlaylist, maxFilesInNewMusicPlaylist) :
                                   CreateStandardPlaylist(mp3sForPlaylist, existingPlaylist);

            var playlistTextFile = GetPlaylistFilesStringForPlaylist(newPlaylist);

            try
            {
                File.WriteAllText(pathToPlaylist, playlistTextFile);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to write playlist to: '{0}'", pathToPlaylist), ex);
            }

            Log.Debug("Completed writing new playlist to: '{0}'", pathToPlaylist);
        }

        private static List<PlaylistFile> CreateNewMusicPlaylist(
            List<DKRPodcastFileToProcess> mp3sForPlaylist, 
            List<PlaylistFile> existingPlaylist,
            int maxFilesInNewMusicPlaylist)
        {
            var datedExistingPlaylist = GetExistingPlaylistWithDates(existingPlaylist);
            var datedNewPlaylist = GetNewPlaylistWithDates(mp3sForPlaylist);

            var possiblePlaylistFiles = new List<DatedPlaylistFile>();

            possiblePlaylistFiles.AddRange(datedExistingPlaylist);
            possiblePlaylistFiles.AddRange(datedNewPlaylist);

            var mostRecentFiles = possiblePlaylistFiles.DistinctBy(file => file.File)
                                                       .OrderByDescending(file => file.PublishDate)
                                                       .Take(maxFilesInNewMusicPlaylist);

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

            foreach (var playlistEntry in newPlaylist)
            {
                sb.AppendFormat(@"{0}{1}={2}", FileKey, index, playlistEntry.File);
                sb.AppendLine();
                sb.AppendFormat(@"{0}{1}={2}", TitleKey, index, playlistEntry.Title);
                sb.AppendLine();
                sb.AppendFormat(@"{0}{1}={2}", LengthKey, index, playlistEntry.Length);
                sb.AppendLine();

                index++;
            }

            var entries = sb.ToString();

            entries = TrimEnd(entries);

            var formattedPlaylist = string.Format(PlaylistFormat, entries, newPlaylist.Count);

            return formattedPlaylist;
        }

        private static string TrimEnd(string content)
        {
            content = content.TrimEnd('\r', '\n');

            return content;
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
