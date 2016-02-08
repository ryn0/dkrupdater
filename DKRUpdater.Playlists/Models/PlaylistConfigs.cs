using DKRUpdater.Core.FileSystem;
using DKRUpdater.Core.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DKRUpdater.Playlists.Models
{
    public class PlaylistConfigs
    {
        [JsonProperty(PropertyName = "maxInNewMusicPlaylist")]
        public int MaxInNewMusicPlaylist { get; set; }

        public PlaylistConfigs()
        {
            
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("maxInNewMusicPlaylist: '{0}'", MaxInNewMusicPlaylist);

            return sb.ToString();
        }

        public static PlaylistConfigs BuildFromConfigs()
        {
            var pathToConfigs = string.Format(@"{0}\playlist_configs.json", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));

            if (!File.Exists(pathToConfigs))
            {
                Log.Debug("Configs at: '{0}' not found, using defaults.", pathToConfigs);

                return ReturnDefault();
            }

            Log.Debug("Starting to load playlist configuration from: '{0}'...", pathToConfigs);

            try
            {
                var json = FileOperations.GetJsonFromPath(pathToConfigs);

                Log.Debug("Completed load of playist configs.");

                Log.Debug("Starting to deserialize playlist configs...");

                var configs = JsonConvert.DeserializeObject<PlaylistConfigs>(json);

                Log.Debug("Completed playlist configs deserialization.");

                Log.Debug(@"Playlist configs are: 
{0}", configs.ToString());


                return configs;
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                return ReturnDefault();
            }
        }

        private static PlaylistConfigs ReturnDefault()
        {
            return new PlaylistConfigs()
            {
                MaxInNewMusicPlaylist = 12
            };
        }
    }
}
