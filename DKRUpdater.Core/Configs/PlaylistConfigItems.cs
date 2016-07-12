using Newtonsoft.Json;

namespace DKRUpdater.Core.Configs
{
    public class PlaylistConfigItems
    {
        [JsonProperty(PropertyName = "maxInNewMusicPlaylist")]
        public int MaxInNewMusicPlaylist { get; set; }

        [JsonProperty(PropertyName = "mp3DownloadDirectory")]
        public string Mp3DownloadDirectory { get; set; }

        [JsonProperty(PropertyName = "mp3LogsDirectory")]
        public string Mp3LogsDirectory { get; set; }

        [JsonProperty(PropertyName = "mp3MusicDirectory")]
        public string Mp3MusicDirectory { get; set; }

        [JsonProperty(PropertyName = "mp3PlaylistsDirectory")]
        public string Mp3PlaylistsDirectory { get; set; }

        [JsonProperty(PropertyName = "newMusicPlaylist")]
        public string NewMusicPlaylist { get; set; } 
    }
}
