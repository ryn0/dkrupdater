using DKRUpdater.Core.FileSystem;
using DKRUpdater.Core.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DKRUpdater.Core.Configs
{
    public static class PlaylistConfigs
    {
        private static readonly PlaylistConfigItems _playlistConfigItems;

        static PlaylistConfigs()
        {
            var pathToConfigs = string.Format(@"{0}\playlist_configs.json", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));

            if (!File.Exists(pathToConfigs))
            {
                Log.Debug("Configs at: '{0}' not found, using defaults.", pathToConfigs);

                throw new Exception("no playlist configs");
            }

            var json = FileOperations.GetJsonFromPath(pathToConfigs);
            _playlistConfigItems = JsonConvert.DeserializeObject<PlaylistConfigItems>(json);
        }
        
        public static int MaxInNewMusicPlaylist
        {
            get
            {
                return _playlistConfigItems.MaxInNewMusicPlaylist;
            }
        }

        public static string Mp3DownloadDirectory
        {
            get
            {
                return _playlistConfigItems.Mp3DownloadDirectory;
            }
        }

        public static string Mp3LogsDirectory
        {
            get
            {
                return _playlistConfigItems.Mp3LogsDirectory;
            }
        }

        public static string Mp3MusicDirectory
        {
            get
            {
                return _playlistConfigItems.Mp3MusicDirectory;
            }
        }

        public static string Mp3PlaylistsDirectory
        {
            get
            {
                return _playlistConfigItems.Mp3PlaylistsDirectory;
            }
        }

        public static string NewMusicPlaylist
        {
            get
            {
                return _playlistConfigItems.NewMusicPlaylist;
            }
        }

        public static bool TrimSilence
        {
            get
            {
                return _playlistConfigItems.TrimSilence;
            }
        }

    }
}
